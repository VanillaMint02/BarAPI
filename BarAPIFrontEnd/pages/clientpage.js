import { useState } from "react";
import styles from "./../styles/Home.module.css";

export default function Home() {
  
  const [showLogin, setShowLogin] = useState(false);
  const [showRegister, setShowRegister] = useState(false);
  return (
    
    <div className={styles.background}>
      
      <div className={styles.center}>0</div>
     <div className={styles.title}>
       <p>Bartender's Controller App</p>
       <button className={styles.login}>Soft Drinks</button>
       <button className={styles.login}>Coffe</button>
       <button className={styles.login}>Shots</button>
       <button className={styles.login}>Cocktails</button>
       
     </div>
     
     
     <div className={styles.leftside}>
     <form className={styles.formleft}>
      <input></input>
      <input></input>
      <button className={styles.formbutton}>Confirm</button>

       </form>
     </div>
    
    <div className={styles.container}>
    
     <div className={styles.loginregisterform}>
      <button
        className={styles.login}
        onClick={() => {
          setShowRegister(false);
          setShowLogin(true);
        }}
      >
        Login
      </button>
      <button className={styles.login}
        onClick={() => {
          setShowRegister(true);
          setShowLogin(false);
        }}
      >
        Register
      </button>
      
      </div>
      
    </div>
    {showLogin && (
        <form className={styles.form}>
          <input placeholder="Username" />
          <input placeholder="Password" type="password" />
          <button className={styles.formbutton}onClick={() => setShowLogin(false)}>Cancel</button>
          <button className={styles.formbutton} >Login</button>
        </form>
      )}
      {showRegister && (
        <form className={styles.form}>
          <input placeholder="Username" />
          <input placeholder="Password" type="password" />
          <input placeholder="Mail" />
          <input placeholder="Confirm mail" />
          <button className={styles.formbutton} onClick={() => setShowRegister(false)}>Cancel</button>
          <button className={styles.formbutton}>SignUp</button>
        </form>
      )}
      
    </div>
  );
 
}
