import axios from "axios";
import React, { Component } from "react";
import authService from "../api-authorization/AuthorizeService";
import Loading from "../loading";

export class CartInfo extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isAuthenticated: false,
      customerId: null,
      cart: [],
      cartItems: [],
      products: [],
      loading: true
    };
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
      .then(response => this.setState({ cart: response.data }))
      .catch(error => console.log(error));
    await axios
      .get("ShoppingCart/getproductincart/" + this.state.cart.cartId)
      .then(response => this.setState({ products: response.data }))
      .catch(error => console.log(error));
    await axios
      .get("ShoppingCart/getcartitems/" + this.state.cart.cartId)
      .then(response =>
        this.setState({ cartItems: response.data, loading: false })
      )
      .catch(error => console.log(error));
  }

  componentWillUnmount() {
    authService.unsubscribe(this._subscription);
  }
  render() {
    let cart = this.state.loading ? (
      <Loading />
    ) : (
      <div>
        <h1>Shopping Cart</h1>
        <table className="table table-striped">
          <thead>
            <tr>
              <th>Product</th>
              <th>Quantity</th>
              <th>Price</th>
              <th>Photo</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {this.state.products.map(product => (
              <tr key={product.productId}>
                <td>{product.name}</td>
                <td>
                  {
                    this.state.cartItems.filter(
                      item => item.productRefId === product.productId
                    )[0].quantity
                  }
                </td>

                <td>{product.price}</td>
                <td>
                  <img
                    id="productphoto"
                    style={{
                      width: 50 + "px",
                      height: 50 + "px",
                      objectFit: "cover"
                    }}
                    src={`WebImage/GetImage/${product.photo}`}
                  />
                </td>

                <td />
              </tr>
            ))}
          </tbody>
        </table>
        <table className="table table-striped">
          <thead>
            <tr>
              <td />
              <td />
              <td />
              <td>Total</td>
              <td>Action</td>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td />
              <td />
              <td />
              <td />
              <td />
            </tr>
          </tbody>
        </table>
      </div>
    );
    return <div>{cart}</div>;
  }
}
