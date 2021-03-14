import React, { useContext, useEffect, useState } from "react";
import { DataContext } from "../../../contexts/DataContext";
import { FormHandlerContext } from "../../../contexts/FormHandlerContext";

const CategoriesTable = () => {
  const { categories, removeItem, currPage, changePage } = useContext(
    DataContext
  );
  const { onEditCategory } = useContext(FormHandlerContext);
  const [pageItems, setPageItems] = useState([]);
  const [findString, setFindString] = useState("");

  useEffect(() => {
    let finish = currPage * 4;
    let start = currPage - 1;
    if (categories.length !== 0) {
      if (!!findString) {
        setPageItems(
          categories
            .filter((categ) =>
              categ.name.toLowerCase().includes(findString.toLowerCase())
            )
            .slice(start * 4, finish)
        );
        if (currPage !== 1) {
          changePage(1);
        }
      } else {
        setPageItems(categories.slice(start * 4, finish));
      }
    }
  }, [categories, currPage, findString, changePage]);

  useEffect(() => {
    setFindString("");
  }, [categories]);

  const findItems = (event) => {
    let name = event.target.value;
    setFindString(name);
  };

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
