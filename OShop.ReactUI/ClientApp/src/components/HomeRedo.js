import React, { useContext, useEffect, useState } from "react";
import { NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import Pagination from "./Pagination";
import "./Home.css";
import { DataContext } from "../contexts/DataContext";

const Home = () => {
  const { products, currPage, changePage } = useContext(DataContext);
  const [pageItems, setPageItems] = useState([]);
  const [findString, setFindString] = useState("");

  const applyStock = (stock) => {
    if (stock > 5) return <div style={{ color: "darkblue" }}>In Stock</div>;
    else if (stock === 0)
      return <div style={{ color: "darkred" }}>Not In Stock</div>;
    else return <div style={{ color: "darkorange" }}>Limited Quantity</div>;
  };

  useEffect(() => {
    let finish = currPage * 12;
    let start = currPage - 1;
    if (products.length !== 0) {
      if (!!findString) {
        setPageItems(
          products
            .filter((prod) =>
              prod.name.toLowerCase().includes(findString.toLowerCase())
            )
            .slice(start * 12, finish)
        );
        if (currPage !== 1) {
          changePage(1);
        }
      } else {
        setPageItems(products.slice(start * 12, finish));
      }
    }
  }, [products, currPage, findString, changePage]);

  const findItems = (event) => {
    let name = event.target.value;
    setFindString(name);
  };
  // const addtocart = async (event) => {
  //   event.preventDefault();
  //   const form = new FormData(event.target);
  //   await axios
  //     .post("/ShoppingCart/addcartitem", form)
  //     .then(() => toast.success("Added to cart", { autoClose: 2000 }))
  //     .catch(() => toast.error("Already in cart", { autoClose: 2000 }));
  // };

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
              <form onSubmit={null}>
                <input
                  type="hidden"
                  name="ProductRefId"
                  value={product.productId}
                />
                <input type="hidden" name="CartRefId" value={null} />
                <input type="hidden" name="Quantity" value="1" />
                <input type="hidden" name="Price" value={null} />
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
