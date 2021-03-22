import React, { useContext, useEffect, useState } from "react";
import { CartContext } from "../contexts/CartContext";
import { DataContext } from "../contexts/DataContext";
import axios from "axios";
import "./Home.css";

const ViewProduct = () => {
  const { products, location, toast } = useContext(DataContext);
  const { cart, toggleReload } = useContext(CartContext);
  const [displayProduct, setDisplayProduct] = useState([]);

  useEffect(() => {
    if (!!location.query.id) {
      let productId = location.query.id;

      setDisplayProduct(
        products.filter((prod) => prod.productId === productId)[0]
      );
    }
  }, [location.query.id, products]);

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

  return displayProduct.length !== 0 ? (
    <div>
      <h1 style={{ textAlign: "center" }}>{displayProduct.name}</h1>
      <div className="container">
        <div id="productviewleft" className="col-lg-6 col-md-6 col-sm-6">
          <img
            style={{
              width: 130 + "px",
              height: 130 + "px",
              objectFit: "cover",
            }}
            src={`WebImage/GetImage/${displayProduct.photo}`}
            alt="product"
          />
        </div>
        <div id="productviewright" className="col-lg-6 col-md-6 col-sm-6">
          <div style={{ fontSize: 20 + "px" }}>
            Product Description: {displayProduct.description}
          </div>
          {applyStock(displayProduct.stock)}
          <div style={{ fontSize: 20 + "px", color: "darkblue" }}>
            Unit Price: {displayProduct.price} $
          </div>
          <div>
            {displayProduct.stock !== 0 ? (
              <form onSubmit={(e) => addtocart(e)}>
                <input
                  type="hidden"
                  name="ProductRefId"
                  value={displayProduct.productId}
                />
                <input type="hidden" name="CartRefId" value={cart.cartId} />
                <input type="hidden" name="Quantity" value="1" />
                <input
                  type="hidden"
                  name="Price"
                  value={displayProduct.price}
                />
                <button
                  className="btn btn-outline-primary btn-sm"
                  type="submit"
                >
                  Add To Cart
                </button>
              </form>
            ) : null}
          </div>
        </div>
      </div>
    </div>
  ) : null;
};

export default ViewProduct;
