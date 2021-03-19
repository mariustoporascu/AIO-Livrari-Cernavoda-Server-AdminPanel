import React, { useContext } from "react";
import { NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import Pagination from "./Pagination";
import "./Home.css";
import axios from "axios";
import { PaginationContext } from "../contexts/PaginationContext";
import { DataContext } from "../contexts/DataContext";
import { CartContext } from "../contexts/CartContext";

const Home = () => {
  const { pageItems } = useContext(PaginationContext);
  const { findItems, toast } = useContext(DataContext);
  const { cart, toggleReload } = useContext(CartContext);

  const applyStock = (stock) => {
    if (stock > 5) return <div style={{ color: "darkblue" }}>In Stock</div>;
    else if (stock === 0)
      return <div style={{ color: "darkred" }}>Not In Stock</div>;
    else return <div style={{ color: "darkorange" }}>Limited Quantity</div>;
  };

  const addtocart = async (event) => {
    event.preventDefault();

    const form = new FormData(event.target);
    await axios
      .post("/ShoppingCart/addcartitem", form)
      .then(() => [
        toast.success("Added to cart", { autoClose: 2000 }),
        toggleReload(),
      ])
      .catch(() => toast.error("Already in cart", { autoClose: 2000 }));
  };

  return (
    <div>
      <input
        className="text-center"
        type="text"
        placeholder="Search product"
        onChange={findItems}
      />
      <div id="main-page-products" className="row">
        {pageItems.map((product) => (
          <div
            key={product.productId}
            id="products"
            className="col-lg-4 col-md-4 col-sm-6"
          >
            <NavLink tag={Link} to="/">
              <img
                style={{
                  width: 130 + "px",
                  height: 130 + "px",
                  objectFit: "cover",
                }}
                src={`WebImage/GetImage/${product.photo}`}
                alt="product"
              />
            </NavLink>
            <NavLink
              tag={Link}
              to="/"
              style={{
                textDecoration: "none",
                fontSize: 20 + "px",
                color: "black",
              }}
            >
              {product.name}
            </NavLink>
            <div style={{ fontSize: 16 + "px", color: "darkblue" }}>
              {product.price} $
            </div>
            {applyStock(product.stock)}
            <div style={{ display: "flex", justifyContent: "center" }}>
              <NavLink
                tag={Link}
                to="/"
                className="btn btn-outline-success btn-sm"
                style={{
                  paddingLeft: 8 + "px",
                  paddingRight: 8 + "px",
                  paddingTop: 4 + "px",
                  paddingBottom: 4 + "px",
                }}
              >
                View Details
              </NavLink>
              <form onSubmit={(e) => addtocart(e)}>
                <input
                  type="hidden"
                  name="ProductRefId"
                  value={product.productId}
                />
                <input
                  type="hidden"
                  name="CartRefId"
                  value={cart.length !== 0 ? cart.cartId : ""}
                />
                <input type="hidden" name="Quantity" value="1" />
                <input type="hidden" name="Price" value={product.price} />
                <button
                  className="btn btn-outline-primary btn-sm"
                  type="submit"
                >
                  Add To Cart
                </button>
              </form>
            </div>
          </div>
        ))}
      </div>
      <Pagination></Pagination>
    </div>
  );
};

export default Home;
