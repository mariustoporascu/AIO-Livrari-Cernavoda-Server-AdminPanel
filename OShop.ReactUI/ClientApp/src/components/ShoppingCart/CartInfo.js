import axios from "axios";
import React, { Component } from "react";
import authService from "../api-authorization/AuthorizeService";

export class CartInfo extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isAuthenticated: false,
      customerId: null,
      cart: [],
      cartItems: [],
      loading: true
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
      .get("ShoppingCart/getcartitems/" + this.state.cart.cartId)
      .then(response =>
        this.setState({ cartItems: response.data, loading: false })
      )
      .catch(error => console.log(error));
  }
  render() {
    return <div>It's working</div>;
  }
}
