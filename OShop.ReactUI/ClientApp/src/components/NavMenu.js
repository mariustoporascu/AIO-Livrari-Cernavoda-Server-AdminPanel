import React, { Component } from "react";
import {
  Collapse,
  Container,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
} from "reactstrap";
import axios from "axios";
import { Link } from "react-router-dom";
import { LoginMenu } from "./api-authorization/LoginMenu";
import "./NavMenu.css";
import authService from "./api-authorization/AuthorizeService";
import cart from "./static/cart.jpg";

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor(props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
      isAuthenticated: false,
      role: null,
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
      role: user && user.role,
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

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed,
    });
  }

  render() {
    let cartCount = this.state.cartItems.length;
    return (
      <header>
        <Navbar
          className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
          light
        >
          <Container>
            <NavbarBrand tag={Link} to="/">
              OShop.ReactUI
            </NavbarBrand>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            <Collapse
              className="d-sm-inline-flex flex-sm-row-reverse"
              isOpen={!this.state.collapsed}
              navbar
            >
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/">
                    Home
                  </NavLink>
                </NavItem>
                {this.state.role && this.state.role.includes("SuperAdmin") ? (
                  <NavItem>
                    <NavLink
                      tag={Link}
                      className="text-dark"
                      to="/adminpanel/manageproducts"
                    >
                      Products
                    </NavLink>
                  </NavItem>
                ) : null}
                {this.state.role && this.state.role.includes("SuperAdmin") ? (
                  <NavItem>
                    <NavLink
                      tag={Link}
                      className="text-dark"
                      to="/adminpanel/managecategories"
                    >
                      Categories
                    </NavLink>
                  </NavItem>
                ) : null}
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/shoppingcart">
                    <img
                      style={{
                        width: 25 + "px",
                        height: 25 + "px",
                        objectFit: "cover",
                      }}
                      src={cart}
                      alt="cart"
                    />
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
                  </NavLink>
                </NavItem>
                <LoginMenu />
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>
    );
  }
}
