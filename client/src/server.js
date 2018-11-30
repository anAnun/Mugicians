import * as axios from "axios";

//axios.defaults.withCredentials = true;

const URL_PREFIX = "";

export function delete_song(id, data) {
  return axios.delete(URL_PREFIX + "/api/song/" + id, data);
}

export function viewAll_get() {
  return axios.get(URL_PREFIX + "/api/songs/");
}

export function song_get(id) {
  return axios.get(URL_PREFIX + "/api/songs/" + id);
}

export function song_update(id, data) {
  return axios.put(URL_PREFIX + "/api/song/" + id, data);
}

export function description_update(id, data) {
  return axios.put(URL_PREFIX + "/api/songfile/" + id, data);
}

export function songs_post(data) {
  return axios.post(URL_PREFIX + "/api/songs/", data);
}

export function file_get(id) {
  return axios.get(URL_PREFIX + "/api/song/" + id + "/files");
}

export function upload_file(id, audio, data) {
  return axios.post(URL_PREFIX + "/api/songfile/" + id, audio, data);
}

export function delete_file(songfile, data) {
  return axios.delete(URL_PREFIX + "/api/songfile/" + songfile, data);
}

export function delete_file_drive(filestring, data) {
  return axios.delete(URL_PREFIX + "/api/songfile/drive/" + filestring + data);
}

// export function uploadFileName_post() {
//   return axios.post(URL_PREFIX + "/api/songfile/" + id, data);
// }
