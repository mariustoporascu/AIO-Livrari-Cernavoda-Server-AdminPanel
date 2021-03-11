import React, { Component } from "react";
import { PaginationContext } from "../../../contexts/PaginationContext";
import { DataContext } from "../../../contexts/DataContext";

class ProductsForm extends Component {
  static contextType = PaginationContext;

  onPost = async (event) => {
    event.preventDefault();
    const { postContent, pageHelper, pageConstr, currPage } = this.context;
    let type;
    await postContent().then((result) => {
      type = result;
    });
    pageHelper();
    const { totalPages } = this.context;
    if (type === "post") {
      let tpLength = totalPages.length;
      if (tpLength > currPage) {
        pageConstr(tpLength);
      } else {
        pageConstr(currPage);
      }
    } else {
      pageConstr(currPage);
    }
  };
  render() {
    return (
      <DataContext.Consumer>
        {(context) => {
          const {
            productId,
            name,
            description,
            stock,
            price,
            categoryRefId,
            categories,
            handleInputs,
            handleFile,
            setCustomValidity,
          } = context;

          let buttonType = !productId ? "Add New" : "Update";
          return (
            <form onSubmit={(e) => this.onPost(e)}>
              <table
                className="table table-striped"
                aria-labelledby="tabelLabel"
              >
                <thead>
                  <tr>
                    <th>Product</th>
                    <th>Input</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <th>* Name</th>
                    <td className="form-group">
                      <input
                        type="text"
                        name="name"
                        value={name}
                        className="form-control"
                        required
                        onChange={(e) => handleInputs(e)}
                        onInvalid={(e) => setCustomValidity(e)}
                      />
                      <span
                        id="span name"
                        className="text-danger field-validation-valid"
                      />
                    </td>
                  </tr>
                  <tr>
                    <th>* Description</th>
                    <td className="form-group">
                      <input
                        type="text"
                        name="description"
                        value={description}
                        className="form-control"
                        required
                        onChange={(e) => handleInputs(e)}
                        onInvalid={(e) => setCustomValidity(e)}
                      />
                      <span
                        id="span description"
                        className="text-danger field-validation-valid"
                      />
                    </td>
                  </tr>
                  <tr>
                    <th>* Stock</th>
                    <td className="form-group">
                      <input
                        type="number"
                        name="stock"
                        min="0"
                        max="10000"
                        value={stock}
                        className="form-control"
                        required
                        onChange={(e) => handleInputs(e)}
                        onInvalid={(e) => setCustomValidity(e)}
                      />
                      <span
                        id="span stock"
                        className="text-danger field-validation-valid"
                      />
                    </td>
                  </tr>
                  <tr>
                    <th>* Price</th>
                    <td className="form-group">
                      <input
                        type="number"
                        step={0.01}
                        min="0.01"
                        max="10000"
                        name="price"
                        value={price}
                        className="form-control"
                        required
                        onChange={(e) => handleInputs(e)}
                        onInvalid={(e) => setCustomValidity(e)}
                      />
                      <span
                        id="span price"
                        className="text-danger field-validation-valid"
                      />
                    </td>
                  </tr>
                  <tr>
                    <th>Photo</th>
                    <td className="form-group">
                      <input
                        type="file"
                        accept="image/png,image/jpg,image/jpeg"
                        name="photo"
                        onChange={handleFile}
                      />
                    </td>
                  </tr>
                  <tr>
                    <th>Category</th>
                    <td className="form-group">
                      <select
                        name="categoryRefId"
                        value={categoryRefId}
                        className="form-control"
                        onChange={(e) => handleInputs(e)}
                      >
                        {categories.map((category) => (
                          <option
                            key={category.categoryId}
                            value={category.categoryId}
                          >
                            {category.name}
                          </option>
                        ))}
                      </select>
                    </td>
                  </tr>
                </tbody>
                <tfoot>
                  <tr>
                    <th>* Required</th>
                  </tr>
                </tfoot>
              </table>
              <button
                type="submit"
                className="btn btn-outline-primary btn-sm"
                style={{
                  float: "right",
                  margin: 10 + "px",
                  padding: 10 + "px",
                }}
              >
                {buttonType}
              </button>
            </form>
          );
        }}
      </DataContext.Consumer>
    );
  }
}

export default ProductsForm;
