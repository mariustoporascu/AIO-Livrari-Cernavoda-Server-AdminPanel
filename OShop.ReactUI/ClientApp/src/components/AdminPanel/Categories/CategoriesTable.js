import React, { useContext } from "react";
import { DataContext } from "../../../contexts/DataContext";
import { FormHandlerContext } from "../../../contexts/FormHandlerContext";
import { PaginationContext } from "../../../contexts/PaginationContext";

const CategoriesTable = () => {
  const { pageItems } = useContext(PaginationContext);
  const { findItems, removeItem } = useContext(DataContext);
  const { onEditCategory } = useContext(FormHandlerContext);

  return (
    <div>
      <input
        className="text-center"
        type="text"
        placeholder="Search category"
        onChange={findItems}
      />
      <table
        className="table table-striped"
        aria-labelledby="tabelLabel"
        style={{
          textAlign: "center",
          border: 2 + "px solid black",
        }}
      >
        <thead>
          <tr>
            <th style={{ textAlign: "left" }}>Name</th>

            <th>Photo</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {pageItems.map((category) => (
            <tr key={category.categoryId}>
              <td style={{ textAlign: "left" }}>{category.name}</td>

              <td>
                <img
                  src={`WebImage/GetImage/${category.photo}`}
                  style={{
                    width: 50 + "px",
                    height: 50 + "px",
                    objectFit: "cover",
                  }}
                  alt="category"
                />
              </td>
              <td>
                <button
                  className="btn btn-outline-warning btn-sm"
                  style={{ marginRight: 10 + "px" }}
                  onClick={() => onEditCategory(category)}
                >
                  Edit
                </button>
                <button
                  className="btn btn-outline-danger btn-sm"
                  onClick={() => removeItem(category.categoryId)}
                >
                  Remove
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default CategoriesTable;
