import React, { Component } from "react";
import NavMenu from "./NavMenu";
import { Container } from "reactstrap";
import CartContextProvider from "../contexts/CartContext";
import DataContextProvider from "../contexts/DataContext";
import PaginationContextProvider from "../contexts/PaginationContext";

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <DataContextProvider>
        <PaginationContextProvider>
          <CartContextProvider>
            <NavMenu />

            <Container>{this.props.children}</Container>
          </CartContextProvider>
        </PaginationContextProvider>
      </DataContextProvider>
    );
  }
}
