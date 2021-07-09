import axios from "axios";

export const addProductToCart = ({ state, commit }, product) => {
    const index = state.cart.findIndex(
        i =>
            i.variant.productId === product.variant.productId &&
            i.variant.colourId === product.variant.colourId &&
            i.variant.storageId === product.variant.storageId &&
            i.image.imageURL === product.image.imageURL
    );
    if (index >= 0) {
        commit("updateProductQuantity", index);
    } else {
        commit("addProductToCart", product);
    }
};

export const removeProductFromCart = ({ state, commit }, product) => {
    const index = state.cart.findIndex(
        i =>
            i.variant.productId === product.variant.productId &&
            i.variant.colourId === product.variant.colourId &&
            i.variant.storageId === product.variant.storageId &&
            i.image.imageURL === product.image.imageURL
    );
  commit("removeProductFromCart", index);
};

export const setProductQuantity = ({ state, commit }, payload) => {
    const index = state.cart.findIndex(
        i =>
            i.variant.productId === payload.product.variant.productId &&
            i.variant.colourId === payload.product.variant.colourId &&
            i.variant.storageId === payload.product.variant.storageId &&
            i.image.imageURL === payload.product.image.imageURL
    );
  if (payload.quantity > 0) {
      commit("setProductQuantity", { cartPayload: payload, cartIndex: index });
  } else {
      commit("removeProductFromCart", index);
  }
};

export const login = ({ commit }, payload) => {
  return new Promise((resolve, reject) => {
    commit("loginRequest");
    axios
      .post("/api/token", payload)
      .then(response => {
        const auth = response.data;
        axios.defaults.headers.common["Authorization"] = `Bearer ${
          auth.access_token
        }`;
        commit("loginSuccess", auth);
        commit("hideAuthModal");
        resolve(response);
      })
      .catch(error => {
        commit("loginError");
        delete axios.defaults.headers.common["Authorization"];
        reject(error.response);
      });
  });
};

export const register = ({ commit }, payload) => {
  return new Promise((resolve, reject) => {
    commit("registerRequest");
    axios
      .post("/api/account", payload)
      .then(response => {
        commit("registerSuccess");
        resolve(response);
      })
      .catch(error => {
        commit("registerError");
        reject(error.response);
      });
  });
};

export const logout = ({ commit }) => {
  commit("logout");
  delete axios.defaults.headers.common["Authorization"];
};
