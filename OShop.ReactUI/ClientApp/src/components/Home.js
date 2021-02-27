import React, { Component } from "react";
import { NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import "./Home.css";
import axios from "axios";
import Pagination from "./Pagination";
import authService from "./api-authorization/AuthorizeService";
import Loading from "./loading";

export class Home extends Component {
  static displayName = Home.name;
  constructor(props) {
    super(props);
    this.state = {
      itemsArray: [],
      findArray: [],
      pageItems: [],
      totalPages: [],
      cartId: null,
      isAuthenticated: false,
      customerId: null,
      currPage: null,
      loading: true
    };
    this.changePage = this.changePage.bind(this);
    this.find = this.find.bind(this);
  }
  componentDidMount() {
    this._subscription = authService.subscribe(() => this.populateState());
    this.populateState();
  }
  async populateState() {
    const [isAuthenticated, user] = await Promise.all([
      authService.isAuthenticated(),
      authService.getUser()
    ]);
    await this.setState({
      isAuthenticated,
      customerId: user && user.sub
    });
    await axios
      .get("ShoppingCart/getcart/" + this.state.customerId)
      .then(response => this.setState({ cartId: response.data.cartId }))
      .catch(error => console.log(error));
    await axios
      .get("/Home/getallproducts")
      .then(response => {
        this.setState({ itemsArray: response.data });
      })
      .catch(error => console.log(error));
    await axios.get("");
    await this.setState({
      totalPages: Pagination.pageHelper(this.state.itemsArray, 12)
    });
    var pagination = Pagination.pagination(1, this.state.itemsArray, 12);
    await this.setState({
      currPage: 1,
      pageItems: pagination.filter,
      loading: false
    });
  }
  componentWillUnmount() {
    authService.unsubscribe(this._subscription);
  }
  static applyStock(stock) {
    if (stock > 5) return <div style={{ color: "darkblue" }}>In Stock</div>;
    else if (stock === 0)
      return <div style={{ color: "darkred" }}>Not In Stock</div>;
    else return <div style={{ color: "darkorange" }}>Limited Quantity</div>;
  }
  async changePage(pageNmbr) {
    var pagination = Pagination.pagination(pageNmbr, this.state.itemsArray, 12);
    await this.setState({ currPage: pageNmbr, pageItems: pagination.filter });
  }

  async find(event) {
    var value = event.target.value;
    await this.setState({ loading: true });
    if (!!value) {
      await this.setState({
        findArray: this.state.itemsArray.filter(product =>
          product.name.toLowerCase().includes(value.toLowerCase())
        )
      });
      await this.setState({
        totalPages: Pagination.pageHelper(this.state.findArray, 12)
      });
      let pagination = Pagination.pagination(1, this.state.findArray, 12);
      await this.setState({
        currPage: 1,
        pageItems: pagination.filter,
        loading: false
      });
    } else {
      await this.setState({
        totalPages: Pagination.pageHelper(this.state.itemsArray, 12)
      });
      let pagination = Pagination.pagination(1, this.state.itemsArray, 12);
      await this.setState({
        currPage: 1,
        pageItems: pagination.filter,
        loading: false
      });
    }
  }

  static addtocart = async event => {
    event.persist();
    event.preventDefault();
    const form = new FormData(event.target);

    await axios
      .post("/ShoppingCart/addcartitem", form)
      .then(result => console.log(result))
      .catch(error => console.log(error));
    alert("Added to cart");
  };

  static productsTable(state) {
    return (
      <div id="main-page-products" className="row">
        {state.pageItems.map(product => (
          <div
            key={product.productId}
            id="products"
            className="col-lg-4 col-md-4 col-sm-6"
          >
            <NavLink tag={Link} to="/">
              <img
                style={{
                  width: 130 + "px",
                  height: 130 + "px",
                  objectFit: "cover"
                }}
                src={`WebImage/GetImage/${product.photo}`}
              />
            </NavLink>
            <NavLink
              tag={Link}
              to="/"
              style={{
                textDecoration: "none",
                fontSize: 20 + "px",
                color: "black"
              }}
            >
              {product.name}
            </NavLink>
            <div style={{ fontSize: 16 + "px", color: "darkblue" }}>
              {product.price} $
            </div>
            {Home.applyStock(product.stock)}
            <div style={{ display: "flex", justifyContent: "center" }}>
              <NavLink
                tag={Link}
                to="/"
                className="btn btn-outline-success btn-sm"
                style={{
                  paddingLeft: 8 + "px",
                  paddingRight: 8 + "px",
                  paddingTop: 4 + "px",
                  paddingBottom: 4 + "px"
                }}
              >
                View Details
              </NavLink>
              <form onSubmit={Home.addtocart}>
                <input
                  type="hidden"
                  name="ProductRefId"
                  value={product.productId}
                />
                <input type="hidden" name="CartRefId" value={state.cartId} />
                <input type="hidden" name="Quantity" value="1" />
                <input type="hidden" name="Price" value={product.price}/>
                <button
                  className="btn btn-outline-primary btn-sm"
                  type="submit"
                >
                  Add To Cart
                </button>
              </form>
            </div>
          </div>
        ))}
      </div>
    );
  }
  render() {
    let pagination = (
      <Pagination counterStates={this.state} passChangePage={this.changePage} />
    );
    let contents = this.state.loading ? (
      <Loading />
    ) : (
      Home.productsTable(this.state)
    );
    return (
      <div>
        <div className="row">
          <input
            type="text"
            name="find"
            placeholder="Search product"
            onChange={this.find}
          />
        </div>
        {contents}
        {pagination}
      </div>
    );
  }
}
