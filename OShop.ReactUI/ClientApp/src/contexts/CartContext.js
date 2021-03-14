import React, { createContext, useContext, useEffect, useState } from "react";
import authService from "../api-authorization/AuthorizeService";

export const CartContext = createContext();

const CartContextProvider = (props) => {
  const [subscription, setSubscription] = useState(0);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState(null);
  const [cart, setCart] = useState(null);
  const [productsInCart, setProductsInCart] = useState(null);
  const [cartItems, setCartItems] = useState(null);

  const populateState = async () => {
    // const [isAuthenticated, user] = await Promise.all([
    //   authService.isAuthenticated(),
    //   authService.getUser(),
    // ]);
    setIsAuthenticated(await Promise(authService.isAuthenticated()));
    setUser((await Promise(authService.getUser())).sub);
    // this.setState({
    //   isAuthenticated,
    //   customerId: user && user.sub,
    // });
    await axios
      .get("ShoppingCart/getcart/" + user)
      .then((response) => setCart(response.data))
      .catch((error) => console.log(error));
    await axios
      .get("ShoppingCart/getproductincart/" + cart.cartId)
      .then((response) => setProductsInCart(response.data))
      .catch((error) => console.log(error));
    await axios
      .get("ShoppingCart/getcartitems/" + cart.cartId)
      .then((response) => setCartItems(response.data))
      .catch((error) => console.log(error));
  };

  useEffect(() => {
    setSubscription(authService.subscribe(() => this.populateState()));
    populateState();
    return authService.unsubscribe(subscription);
  }, []);

  return <CartContext.Provider value={{}}></CartContext.Provider>;
};

export default CartContextProvider;
