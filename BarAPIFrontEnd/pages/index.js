import { getMiddlewareManifest } from "next/dist/client/route-loader";
import { useState, useRef } from "react";
import styles from "./../styles/Home.module.css";

export default function Home() {
  const[showLogin,setShowLogin]=useState(true);
  const [showControllers, setShowControllers] = useState(false);
  const [patchResponse, setPatchResponse] = useState([]);
  const [tick, setTick] = useState(false);
  //for posting ingredients
  const ingredientNameRef = useRef();
  const ingredientIDRef = useRef();
  const availabilityRef = useRef();
  // for patching ingredients
  const ingredientPatch = useRef();
  const ingredientIDPatch = useRef();
  //for product
  const productNameRef = useRef();
  const ID_ProductRef = useRef();
  const priceRef = useRef();
  // for junction
  const junctionIDRef = useRef();
  const productIDRef = useRef();
  const ID_IngredientRef = useRef();
  const rProductNameRef=useRef();
  const rIngredientNameRef=useRef();

  function getSoftDrinks() {
    fetch("http://localhost:5000/api/product/0")
      .then((response) => response.json())
      .then((data) => setPatchResponse(data));
  }
  function getCoffe() {
    fetch("http://localhost:5000/api/product/1")
      .then((response) => response.json())
      .then((data) => setPatchResponse(data));
  }
  function getShots() {
    fetch("http://localhost:5000/api/product/2")
      .then((response) => response.json())
      .then((data) => setPatchResponse(data));
  }
  function getCocktails() {
    fetch("http://localhost:5000/api/product/3")
      .then((response) => response.json())
      .then((data) => setPatchResponse(data));
  }
  function getIngredients() {
    fetch("http://localhost:5000/api/ingredient")
      .then((response) => response.json())
      .then((data) => setPatchResponse(data));
  }

  function verifyPassword() {
    let variable = document.getElementById("passwordInput").value;
    if (variable == "alprazolam") {
      alert("Password good");
      return true;
    } else {
      alert("Password bad");
      return false;
    }
  }

  function postProduct() {
    fetch("http://localhost:5000/api/product", {
      method: "POST",
      body: JSON.stringify({
        productName: productNameRef.current.value,
        ID_Product: productIDRef.current.value,
        price: ID_IngredientRef.current.value,
      }),
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((response) => response.json())
      .then(alert("Posted Succesfully"));
  }
  function postJunction() {

    fetch("http://localhost:5000/api/junction/?pr=Cafea_anericana&ig=lime", {
      method: "POST",
      body: JSON.stringify({
        productName: rProductNameRef,
        ingredientName: rIngredientNameRef,
      }),
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((response) => response.json())
      .then(alert("Posted Succesfully"));
      
  }

  function postIngredients() {
    fetch("http://localhost:5000/api/ingredient", {
      method: "POST",
      body: JSON.stringify({
        ID_Ingredient: ingredientIDRef.current.value,
        IngredientName: ingredientNameRef.current.value,
        Availability: availabilityRef.current.value,
      }),
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((response) => response.json())
      .then(alert("Posted Succesfully"));
  }
  function changeStatus() {
    fetch("http://localhost:5000/api/ingredient", {
      method: "PATCH",
      body: JSON.stringify({
        IngredientName: ingredientPatch.current.value,
        Availability: ingredientIDPatch.current.value,
      }),
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((response) => response.json())
      .then(alert("Patched Succesfully"));
  }
  return (
    <div className={styles.background}>
      {tick && (
        <>
          <div className={styles.center} id="whiteboard">
            {patchResponse?.map((response) => (
              <div key={response.ingredientName}>
                <p>
                  {response.ingredientName}  {response.ID_Ingredient}  {response.Availability}
                </p>
              </div>
            ))}
          </div>
        </>
      )}
      {!tick && (
        <>
          <div className={styles.center} id="whiteboard">
            {patchResponse?.map((response) => (
              <div key={response.Product_Name}>
                <p>
                  {response.Product_Name} {response.Price}
                </p>
              </div>
            ))}
          </div>
        </>
      )}
      <div className={styles.title}>
        <p>Menu</p>
        <button
          className={styles.login}
          onClick={() => {
            setTick(false);
            getSoftDrinks();
          }}
        >
          Soft Drinks
        </button>
        <button
          type="button"
          className={styles.login}
          onClick={() => {
            setTick(false);
            getCoffe();
          }}
        >
          Coffe
        </button>
        <button
          type="button"
          className={styles.login}
          onClick={() => {
            setTick(false);
            getShots();
          }}
        >
          Shots
        </button>
        <button
          type="button"
          className={styles.login}
          onClick={() => {
            setTick(false);
            getCocktails();
          }}
        >
          Cocktails
        </button>
        {showControllers && (
          <>
            <button
              type="button"
              className={styles.login}
              onClick={() => {
                getIngredients();
                setTick(true);
              }}
            >
              Ingredients
            </button>
          </>
        )}
      </div>
      {showControllers && (
        <>
          <div className={styles.leftside}>
            <form className={styles.formleft}>
              <p>Ingredient status controller</p>
              <input
                placeholder="Ingredient Name"
                ref={ingredientPatch}
              ></input>
              <input
                placeholder="Ingredient Status "
                ref={ingredientIDPatch}
              ></input>
              <button
                type="button"
                className={styles.formbutton}
                onClick={changeStatus}
              >
                Confirm
              </button>
              <p>Ingredient adder</p>
              <input
                placeholder="Ingredient Name"
                ref={ingredientNameRef}
              ></input>
              <input placeholder="Ingredient ID" ref={ingredientIDRef}></input>
              <input
                placeholder="Ingredient Availability"
                ref={availabilityRef}
              ></input>
              <button
                type="button"
                className={styles.formbutton}
                onClick={postIngredients}
              >
                Confirm
              </button>
            </form>
          </div>
          <div className={styles.rightside}>
            <form className={styles.formleft}>
              <p>Product Adder</p>
              <input placeholder="Product Name" ref={productNameRef}></input>
              <input placeholder="Product ID" ref={ID_ProductRef}></input>
              <input placeholder="Product Price" ref={priceRef}></input>
              <button
                type="button"
                className={styles.formbutton}
                onClick={postProduct}
              >
                Confirm
              </button>
              <p>Recipe-Maker</p>
              <input placeholder="Product " ref={rProductNameRef}></input>
              <input placeholder="Ingredient " ref={rIngredientNameRef}></input>
              <button
                type="button"
                className={styles.formbutton}
                onClick={postJunction()}
              >
                Confirm
              </button>
            </form>
          </div>
        </>
      )}
      <div className={styles.container}>
        {showLogin && (
          <>
        <div className={styles.loginregisterform}>
          <input
            placeholder="Password"
            id="passwordInput"
            input
            type="password"
          ></input>
         
           
          <button
            className={styles.login}
            onClick={() => {
              if (verifyPassword()) { setShowControllers(true);setShowLogin(false);}
            }}
          >
            Login
          </button>
          
        
        </div>
        </>
        )}
      </div>
    </div>
  );
}
