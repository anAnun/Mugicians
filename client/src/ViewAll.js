import React from "react";
import { viewAll_get } from "./server";
import { withRouter } from "react-router-dom";

class ViewAll extends React.Component {
  state = {
    id: "",
    songs: [],
    error: "",
    songName: "",
    lyrics: ""
  };

  edit = () => {
    this.setState({
      redirect: true
    });
  };

  componentDidMount() {
    const myPromise = viewAll_get();
    myPromise.then(
      resp => {
        for (let i = 0; i < resp.data.length; i++) {
          resp.data.sort(
            function(a, b) {
              if (a.songName < b.songName) return -1;
              if (a.songName > b.songName) return 1;
              return 0;
            },
            this.setState({
              songs: resp.data
            })
          );
        }
      }
      //err => this.setState({ error: String(err) })
    );
  }

  render() {
    if (this.state.songs) {
      return (
        <div>
          <div className="input">
            <div>
              {this.state.songs.map(song => (
                <div key={song.id}>
                  <button
                    className="songButton"
                    onClick={() => this.props.history.push("/" + song.id)}
                  >
                    {song.songName}
                  </button>
                </div>
              ))}
            </div>
            <br />

            {/* <audio
              controls
              src={
                "https://drive.google.com/uc?export=download&id=" +
                "1I5Y6K7ZLQG9vdSlMo50M1OXKCNiD-8et" +
                "'"
              }
              //https://drive.google.com/file/d/1I5Y6K7ZLQG9vdSlMo50M1OXKCNiD-8et/view?usp=sharing
            /> */}
          </div>
        </div>
      );
    }
  }
}
export default withRouter(ViewAll);
