/*eslint unicode-bom: ["error", "always"]*/
import axios from "axios";
import React, { Component } from "react";
import authService from "../api-authorization/AuthorizeService";
import Loading from "../loading";
import OrderInfo from "./OrderInfo";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

toast.configure();

export class CartInfo extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isAuthenticated: false,
      customerId: null,
      quantitySelection: [
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13,
        14,
        15,
        16,
        17,
        18,
        19,
        20,
      ],
      selectedQuantity: null,
      cart: [],
      cartItems: [],
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
      .then((response) =>
        this.setState({ cartItems: response.data, loading: false })
      )
      .catch((error) => console.log(error));
  }
  updateQuantity = async (event) => {
    var productRefId = parseInt(event.target.name);
    var newQuantity = parseInt(event.target.value);
    var price = parseFloat(event.target.id);
    this.setState({ loading: true });

    var cartIndex = this.state.cartItems.findIndex(
      (obj) => obj.productRefId === productRefId
    );

    var prevCartItem = this.state.cartItems[cartIndex];

    const form = new FormData();
    form.append("cartRefId", this.state.cart.cartId);
    form.append("productRefId", productRefId);
    form.append("quantity", newQuantity);
    form.append("price", price);
    form.append("prevQuantity", prevCartItem.quantity);
    var newTotal =
      this.state.cart.totalInCart +
      (newQuantity - prevCartItem.quantity) * price;
    this.setState((prevState) => ({
      cartItems: prevState.cartItems.map((obj) =>
        obj.productRefId === productRefId
          ? Object.assign(obj, { quantity: parseInt(newQuantity) })
          : obj
      ),
      cart: Object.assign(prevState.cart, { totalInCart: newTotal }),
    }));
    await axios
      .put("ShoppingCart/updatecartitem/", form)
      .then(() => toast.success("Updated", { autoClose: 2000 }))
      .catch(() => toast.error("Error", { autoClose: 2000 }));
    this.setState({ loading: false });
  };

  removeCartItem = async (event) => {
    var cartIndex = this.state.cartItems.findIndex(
      (obj) => obj.productRefId === parseInt(event.target.value)
    );
    var cartItem = this.state.cartItems[cartIndex];
    var productPrice = this.state.products[cartIndex].price;
    this.setState({ loading: true });

    await axios
      .delete("ShoppingCart/removecartitem", {
        params: {
          CartRefId: cartItem.cartRefId,
          ProductRefId: cartItem.productRefId,
          Quantity: cartItem.quantity,
          Price: productPrice,
        },
      })
      .then(() => toast.success("Removed", { autoClose: 2000 }))
      .catch(() => toast.error("Error", { autoClose: 2000 }));
    this.state.cartItems.splice(cartIndex, 1);
    this.state.products.splice(cartIndex, 1);
    this.setState({ loading: false });
  };

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
            {this.state.products.map((product) => (
              <tr key={product.productId}>
                <td>{product.name}</td>
                <td>
                  <select
                    id={product.price}
                    name={product.productId}
                    value={
                      this.state.cartItems.filter(
                        (item) => item.productRefId === product.productId
                      )[0].quantity
                    }
                    className="form-select"
                    size="3"
                    onChange={this.updateQuantity}
                  >
                    {this.state.quantitySelection.map((select) => (
                      <option key={select} value={select}>
                        {select}
                      </option>
                    ))}
                  </select>
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

                <td>
                  <button
                    type="submit"
                    value={product.productId}
                    className="btn btn-outline-danger btn-sm"
                    onClick={this.removeCartItem}
                  >
                    Remove
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
          <tfoot>
            <tr>
              <th></th>
              <th></th>
              <th></th>
              <th></th>
              <th>Total: {this.state.cart.totalInCart.toFixed(2)}</th>
            </tr>
          </tfoot>
        </table>
        <OrderInfo customer={this.state.customerId} />
      </div>
    );
    return <div>{cart}</div>;
  }
}
