import * as axios from "axios";

//axios.defaults.withCredentials = true;

const URL_PREFIX = "";

export function viewAll_get() {
  return axios.get(URL_PREFIX + "/api/songs/");
}

export function songs_post(data) {
  return axios.post(URL_PREFIX + "/api/songs", data);
}

// export function uploadFileName_post() {
//   return axios.post(URL_PREFIX + "/api/songfile/" + id, data);
// }
