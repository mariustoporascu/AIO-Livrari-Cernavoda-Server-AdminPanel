/*eslint unicode-bom: ["error", "always"]*/
import axios from "axios";
import React, { Component } from "react";
import authService from "../api-authorization/AuthorizeService";

export class CartItemsForNavbar extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isAuthenticated: false,
      user: null,
      cartId: null,
      cartItems: [],
    };
  }
  componentDidMount() {
    this._subscription = authService.subscribe(() => this.populateState());
    this.populateState();
  }

  componentWillUnmount() {
    authService.unsubscribe(this._subscription);
  }
  async populateState() {
    const [isAuthenticated, user] = await Promise.all([
      authService.isAuthenticated(),
      authService.getUser(),
    ]);
    this.setState({
      isAuthenticated,
      user: user && user.sub,
    });

    await axios
      .get("ShoppingCart/getcart/" + this.state.user)
      .then((response) => this.setState({ cartId: response.data.cartId }))
      .catch((error) => console.log(error));
    await axios
      .get("ShoppingCart/getcartitems/" + this.state.cartId)
      .then((response) => this.setState({ cartItems: response.data }))
      .catch((error) => console.log(error));
  }
  async componentDidUpdate() {
    await axios
      .get("ShoppingCart/getcart/" + this.state.user)
      .then((response) => this.setState({ cartId: response.data.cartId }))
      .catch((error) => console.log(error));
    await axios
      .get("ShoppingCart/getcartitems/" + this.state.cartId)
      .then((response) => this.setState({ cartItems: response.data }))
      .catch((error) => console.log(error));
  }

  render() {
    let cartCount = this.state.cartItems.length;
    return (
      <span
        style={{
          backgroundColor: "yellow",
          fontSize: 0.8 + "em",
          borderRadius: 1 + "em",
          position: "relative",
          top: 1 + "em",
          right: 0.5 + "em",
        }}
      >
        {cartCount}
      </span>
    );
  }
}
