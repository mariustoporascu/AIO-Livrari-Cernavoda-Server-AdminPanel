import React, { useContext, useState } from "react";
import {
  Collapse,
  Container,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
} from "reactstrap";

import { Link } from "react-router-dom";
import { LoginMenu } from "./api-authorization/LoginMenu";
import "./NavMenu.css";
import cart from "./static/cart.jpg";
import { CartContext } from "../contexts/CartContext";

const NavMenu = () => {
  const [collapsed, setCollapsed] = useState(true);
  const { userInfo, isAuthenticatedUser, cartItems } = useContext(CartContext);

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  };

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
          <NavbarToggler onClick={toggleNavbar} className="mr-2" />
          <Collapse
            className="d-sm-inline-flex flex-sm-row-reverse"
            isOpen={!collapsed}
            navbar
          >
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">
                  Home
                </NavLink>
              </NavItem>
              {isAuthenticatedUser &&
              userInfo &&
              userInfo.role &&
              userInfo.role.includes("SuperAdmin") ? (
                <div style={{ display: "inline-flex" }}>
                  <NavItem>
                    <NavLink
                      tag={Link}
                      className="text-dark"
                      to="/adminpanel/manageproducts"
                    >
                      Products
                    </NavLink>
                  </NavItem>
                  <NavItem>
                    <NavLink
                      tag={Link}
                      className="text-dark"
                      to="/adminpanel/managecategories"
                    >
                      Categories
                    </NavLink>
                  </NavItem>
                  <NavItem>
                    <NavLink
                      tag={Link}
                      className="text-dark"
                      to="/adminpanel/manageorders"
                    >
                      Orders
                    </NavLink>
                  </NavItem>
                </div>
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
                    {cartItems.length}
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
};
export default NavMenu;
