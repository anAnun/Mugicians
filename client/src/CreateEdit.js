import React from "react";
import axios from "axios";
import { Redirect } from "react-router-dom";
import { songs_post } from "./server";

class CreateEdit extends React.Component {
  state = {
    songName: "",
    lyrics: "",
    validated: false,
    fileName: "",
    songFiles: [],
    description: ""
  };

  componentDidMount = () => {
    console.log(this.props.match.params.id);
    if (this.props.match.params.id) {
      this.populateForm(this.props.match.params.id);
    }
  };

  uploadFileName = selectorFiles => {
    {
      console.log(selectorFiles);
      var bodyFormData = new FormData();
      bodyFormData.append("audio", selectorFiles[0]);
      const myPromise = axios.post(
        "api/songfile/" + this.props.match.params.id,
        bodyFormData,
        {
          headers: {
            "Content-Type": "multipart/form-data"
          }
        }
      );
      myPromise.then(this.componentDidMount);
    }
  };

  populateForm = () => {
    const myPromise = axios.get("api/songs/" + this.props.match.params.id);
    myPromise.then(e => {
      this.setState({
        id: e.data.id,
        songName: e.data.songName,
        lyrics: e.data.lyrics
      });
    });
    const myPromise2 = axios.get(
      "api/song/" + this.props.match.params.id + "/files"
    );
    myPromise2.then(s => {
      this.setState(
        {
          songFiles: s.data
        },
        () => console.log("so far so good")
      );
    });
  };

  deleteDescriptionConfirm = () => {
    this.setState({ confirm2: true });
  };
  deleteDescription = fileString => {
    const myPromise = axios.delete(
      "api/songfile/" + this.state.toBeDeletedFile,
      {
        Id: this.state.toBeDeletedFile
      }
    );
    axios.delete("api/songfile/drive/" + fileString, {
      fileString: fileString
    });
    myPromise.then(() =>
      this.setState(
        {
          confirm2: false
        },
        this.componentDidMount //slice the array here
      )
    );
    // const array = [...this.state.songFiles];
    // for (let i = 0; i < array.length; i++) {
    //   if (array[i].id === id) {
    //     array.splice(i, 1);
    //     break;
    //   }
    // }
    // this.setState({
    //   songFile: array
    // });
  };

  // sliceForDelete = () => {
  //   const array = [...this.state.songFiles];
  //   for (let i = 0; i < array.length; i++) {
  //     if (array === this.state.toBeDeletedFile) {
  //       array.splice(i, 1);
  //       break;
  //     }
  //   }
  //   this.setState(
  //     {
  //       songFiles: array
  //     },
  //     this.deleteDescription
  //   );
  // };

  validate = shouldDoAjaxCall => {
    this.setState({
      error: false
    });

    if (this.state.songName) {
      this.setState({
        validated: true
      });
      if (shouldDoAjaxCall && this.state.create) {
        this.create();
      }
      if (shouldDoAjaxCall && this.state.update) {
        this.update();
      }
    }
    if (!this.state.songName) {
      this.setState({
        error: true
      });
    }
  };

  submit = () => {
    this.setState(
      {
        create: true,
        update: false
      },
      () => this.validate(true)
    );
  };

  put = () => {
    this.setState(
      {
        update: true,
        create: false
      },
      () => this.validate(true)
    );
  };

  create = () => {
    const myPromise = songs_post({
      SongName: this.state.songName,
      Lyrics: this.state.lyrics
    });
    myPromise.then(() => this.props.history.push("/home"));
  };

  update = () => {
    const myPromise = axios.put("api/song/" + this.props.match.params.id, {
      Id: this.props.match.params.id,
      SongName: this.state.songName,
      Lyrics: this.state.lyrics
    });
    myPromise.then(alert("Update success!"));
  };

  updateDescription = (song, id, description, audioFile) => {
    console.log("song", song, "id", id);
    const myPromise = axios.put("api/songfile/" + id, {
      Id: id,
      Description: description,
      AudioFile: audioFile,
      SongId: this.props.match.params.id
    });
  };

  delete = () => {
    this.setState({
      confirm: true
    });
  };

  deleteCall = () => {
    const myPromise = axios.delete("api/song/" + this.props.match.params.id, {
      Id: this.props.match.params.id
    });
    myPromise.then(() => {
      this.setState(
        {
          confirm: false
        },
        this.props.history.push("/home")
      );
    });
  };

  cancel = () => {
    this.setState({
      confirm: false,
      confirm2: false
    });
  };

  handleDescriptionChange = (newDesc, indexToChange) => {
    const newItems = this.state.songFiles.map((item, index) => {
      if (index === indexToChange) {
        return {
          ...item,
          description: newDesc
        };
      } else {
        return item;
      }
    });
    console.log(newItems, "htis is what is going into desc");
    this.setState({ songFiles: newItems });
  };

  render() {
    return (
      <div className="body">
        {this.state.confirm ? (
          <div className="dim">
            <div className="dim confirm">
              <button className="btn btn-danger" onClick={this.deleteCall}>
                DELETE
              </button>
              <button className="btn btn-primary" onClick={this.cancel}>
                Cancel
              </button>
            </div>
          </div>
        ) : (
          this.state.confirm2 && (
            <div className="dim">
              <div className="dim cconfirm">
                <button
                  className="btn btn-danger"
                  onClick={() =>
                    this.deleteDescription(this.state.toBeDeletedFileString)
                  }
                >
                  DELETE
                </button>
                <button className="btn btn-primary" onClick={this.cancel}>
                  Cancel
                </button>
              </div>
            </div>
          )
        )}
        <div className="container">
          <div>
            <label
              className={
                this.state.error
                  ? " textError content-title-1"
                  : "content-title-1 text"
              }
            >
              {this.state.error ? "Enter title!!" : "Song name: "}
            </label>
            <br />
            <br />
            <input
              className={
                this.state.error
                  ? "error input-songname content-1"
                  : "input-songname content-1"
              }
              value={this.state.songName}
              onChange={e => {
                this.setState(
                  {
                    songName: e.target.value
                  },
                  () => this.validate(false)
                );
              }}
            />
          </div>
          <div>
            <br />
            <label className="text content-title-2">Lyrics: </label>
            <br />
            <br />
            <textarea
              className="area content-2"
              value={this.state.lyrics}
              onChange={e => {
                this.setState({
                  lyrics: e.target.value
                });
              }}
            />
            {!this.props.match.params.id ? (
              <button onClick={this.submit} className="create">
                Create
              </button>
            ) : (
              <div>
                <div>
                  {this.state.songFiles.map((song, i) => (
                    <div key={song.id} className="grid-files">
                      <audio
                        className="audio"
                        controls
                        preload="none"
                        src={
                          "/api/songfile?filename=" +
                          encodeURIComponent(song.audioFile)
                        }
                      />
                      <div className="grid-buttons">
                        <button
                          onClick={() =>
                            this.updateDescription(
                              song,
                              song.id,
                              song.description,
                              song.audioFile
                            )
                          }
                          className="desc-up"
                        >
                          Save
                        </button>
                        <button
                          onClick={() =>
                            this.setState(
                              {
                                toBeDeletedFile: song.id,
                                toBeDeletedFileString: song.audioFile
                              },
                              this.deleteDescriptionConfirm
                            )
                          }
                          className="desc-del"
                        >
                          Delete
                        </button>
                      </div>
                      {console.log(
                        song.description,
                        "song description console.log"
                      )}
                      <textarea
                        value={song.description || ""}
                        className="song-description"
                        onChange={e =>
                          this.handleDescriptionChange(e.target.value, i)
                        }
                      />
                    </div>
                  ))}

                  <h1 className="text-grey">File Upload</h1>
                  <form onSubmit={this.uploadFileName}>
                    <input
                      type="file"
                      className="btn text upload-button"
                      value={this.state.fileName}
                      onChange={e => {
                        this.setState(
                          {
                            fileName: e.target.value
                          },
                          this.uploadFileName(e.target.files)
                        );
                      }}
                    />
                  </form>
                  <button onClick={this.put} className="edit">
                    Update
                  </button>
                  <button onClick={this.delete} className="delete">
                    Delete
                  </button>
                </div>
              </div>
            )}
            <div />
          </div>
        </div>
      </div>
    );
  }
}
export default CreateEdit;
