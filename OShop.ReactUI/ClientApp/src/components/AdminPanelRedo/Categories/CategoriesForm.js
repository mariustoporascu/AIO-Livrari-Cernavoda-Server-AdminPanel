import React, { Component } from "react";
import { PaginationContext } from "../../../contexts/PaginationContext";
import { DataContext } from "../../../contexts/DataContext";

class CategoriesForm extends Component {
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
            categoryId,
            name,
            handleInputs,
            handleFile,
            setCustomValidity,
          } = context;

          let buttonType = !categoryId ? "Add New" : "Update";
          return (
            <form onSubmit={(e) => this.onPost(e)}>
              <table
                className="table table-striped"
                aria-labelledby="tabelLabel"
              >
                <thead>
                  <tr>
                    <th>Category</th>
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

export default CategoriesForm;
