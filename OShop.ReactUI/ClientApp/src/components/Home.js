import React, { Component } from "react";
import { NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import "./Home.css";
import axios from "axios";

export class Home extends Component {
  static displayName = Home.name;
  constructor(props) {
    super(props);
    this.state = { products: [], loading: true };
  }
  async componentDidMount() {
    await axios
      .get("/Home/getallproducts")
      .then(response => {
        this.setState({ products: response.data, loading: false });
      })
      .catch(error => console.log(error));
  }
  applyStock = stock => {
    if (stock > 5) return <div style={{ color: "darkblue" }}>In Stock</div>;
    else if (stock === 0)
      return <div style={{ color: "darkred" }}>Not In Stock</div>;
    else return <div style={{ color: "darkorange" }}>Limited Quantity</div>;
  };

  render() {
    return (
      <div id="main-page-products" className="row">
        {this.state.products.map(product => (
          <div
            key={product.productId}
            id="products"
            className="col-lg-4 col-md-4 col-sm-6"
          >
            <NavLink tag={Link} to="/">
              <img
                alt="productphoto"
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
            {this.applyStock(product.stock)}
            <div style={{ display: "flex", justifyContent: "center" }}>
              <NavLink
                tag={Link}
                to="/"
                className="btn btn-outline-success btn-sm"
              >
                View Details
              </NavLink>
            </div>
          </div>
        ))}
      </div>
    );
  }
}
