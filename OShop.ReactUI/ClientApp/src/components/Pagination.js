import React, { useContext, useEffect } from "react";
import { PaginationContext } from "../contexts/PaginationContext";

const Pagination = () => {
  const { totalPages, currPage, changePage } = useContext(PaginationContext);

  useEffect(() => {
    if (totalPages.length !== 0) {
      if (currPage > totalPages.length) {
        changePage(totalPages.length);
      }
    }
  }, [totalPages, currPage, changePage]);

  return (
    <div style={{ display: "flex", justifyContent: "center" }}>
      {totalPages.map((page) => (
        <button
          key={page}
          className={
            page === currPage
              ? "active btn btn-outline-info btn-sm"
              : "btn btn-outline-info btn-sm"
          }
          style={{
            marginRight: 10 + "px",
          }}
          onClick={() => changePage(page)}
        >
          Page {page}
        </button>
      ))}
    </div>
  );
};

export default Pagination;
