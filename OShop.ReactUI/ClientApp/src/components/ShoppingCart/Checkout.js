import axios from "axios";
import React, { Component } from "react";
import authService from "../api-authorization/AuthorizeService";
import Loading from "../loading";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

toast.configure();

export class Checkout extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isAuthenticated: false,
      customerId: null,
      disabledBtn: true,
      cart: [],
      cartItems: [],
      orderId: 0,
      orderInfo: [],
      products: [],
      loading: true,
    };
  }

  componentDidMount() {
    this._subscription = authService.subscribe(() => this.populateState());
    this.populateState();
  }

  async populateState() {
    const [isAuthenticated, user] = await Promise.all([
      authService.isAuthenticated(),
      authService.getUser(),
    ]);
    this.setState({
      isAuthenticated,
      customerId: user && user.sub,
    });
    await axios
      .get("ShoppingCart/getcart/" + this.state.customerId)
      .then((response) => this.setState({ cart: response.data }))
      .catch((error) => console.log(error));
    await axios
      .get("ShoppingCart/getproductincart/" + this.state.cart.cartId)
      .then((response) => this.setState({ products: response.data }))
      .catch((error) => console.log(error));
    await axios
      .get("ShoppingCart/getcartitems/" + this.state.cart.cartId)
      .then((response) => this.setState({ cartItems: response.data }))
      .catch((error) => console.log(error));
    await axios
      .get("ShoppingCart/orderinfo/" + this.state.cart.customerId)
      .then((response) =>
        this.setState({ orderInfo: response.data, loading: false })
      )
      .catch((error) => console.log(error));
    if (
      !!this.state.orderInfo.firstName &&
      !!this.state.orderInfo.lastName &&
      !!this.state.orderInfo.address &&
      !!this.state.orderInfo.phoneNo
    ) {
      this.setState({ disabledBtn: false });
    }
  }

  placeOrder = async (event) => {
    event.persist();
    event.preventDefault();
    const form = new FormData();
    form.append("orderId", this.state.orderInfo.orderRefId);
    form.append("status", "Ordered");
    form.append("customerId", this.state.cart.customerId);
    form.append("totalOrdered", this.state.cart.totalInCart);
    await axios
      .put("ShoppingCart/placeorder", form)
      .then(() =>
        toast.success("Congratulations, your order was successfull!", {
          autoClose: 2000,
        })
      )
      .catch((error) => console.log(error));
    if (!!this.state.customerId) {
      setTimeout(function () {
        const redirectUrl = `${window.location.origin}/Identity/Account/Manage/Orders`;

        window.location.replace(redirectUrl);
      }, 2000);
    } else {
      setTimeout(function () {
        const redirectUrl = `${window.location.origin}/`;

        window.location.replace(redirectUrl);
      }, 2000);
    }
  };
  redirect() {
    const redirectUrl = `${window.location.origin}/Identity/Account/Manage/Orders`;

    window.location.replace(redirectUrl);
  }

  componentWillUnmount() {
    authService.unsubscribe(this._subscription);
  }

  render() {
    let checkout = this.state.loading ? (
      <Loading />
    ) : (
      <div>
        <h1>Checkout</h1>
        <table className="table table-striped">
          <thead>
            <tr>
              <th>Product</th>
              <th>Quantity</th>
              <th>Price</th>
              <th>Photo</th>
            </tr>
          </thead>
          <tbody>
            {this.state.products.map((product) => (
              <tr key={product.productId}>
                <td>{product.name}</td>
                <td>
                  {
                    this.state.cartItems.filter(
                      (item) => item.productRefId === product.productId
                    )[0].quantity
                  }
                </td>
                <td>
                  {(
                    product.price *
                    this.state.cartItems.filter(
                      (item) => item.productRefId === product.productId
                    )[0].quantity
                  ).toFixed(2)}
                </td>
                <td>
                  <img
                    id="productphoto"
                    style={{
                      width: 50 + "px",
                      height: 50 + "px",
                      objectFit: "cover",
                    }}
                    src={`WebImage/GetImage/${product.photo}`}
                  />
                </td>
              </tr>
            ))}
          </tbody>
          <tfoot>
            <tr>
              <th></th>
              <th></th>
              <th></th>

              <th>Total: {this.state.cart.totalInCart.toFixed(2)}</th>
            </tr>
          </tfoot>
        </table>
        <table className="table table-striped">
          <thead></thead>
          <tbody>
            <tr>
              <th>First Name</th>
              <td>{this.state.orderInfo.firstName}</td>
            </tr>
            <tr>
              <th>Last Name</th>
              <td>{this.state.orderInfo.lastName}</td>
            </tr>
            <tr>
              <th>Address</th>
              <td>{this.state.orderInfo.address}</td>
            </tr>
            <tr>
              <th>Phone No</th>
              <td>{this.state.orderInfo.phoneNo}</td>
            </tr>
          </tbody>
        </table>
      </div>
    );
    let showSpan = this.state.disabledBtn ? (
      <span
        className="d-inline-block"
        tabIndex="0"
        data-toggle="tooltip"
        title="You must add order info to proceed"
        style={{ float: "right" }}
      >
        <button
          disabled="true"
          className="btn btn-success"
          style={{ margin: 1 + "em", float: "right" }}
          onClick={this.placeOrder}
        >
          Place Order
        </button>
      </span>
    ) : (
      <button
        className="btn btn-success"
        style={{ margin: 1 + "em", float: "right" }}
        onClick={this.placeOrder}
      >
        Place Order
      </button>
    );
    return (
      <div>
        {checkout}
        {showSpan}
        <a
          className="btn btn-warning"
          style={{ margin: 1 + "em", float: "right" }}
          href="/shoppingcart"
        >
          Go Back
        </a>
      </div>
    );
  }
}
