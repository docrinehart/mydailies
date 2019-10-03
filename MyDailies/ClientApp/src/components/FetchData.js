import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { ratings: [], loading: true };
  }

  componentDidMount() {
    this.populateRatings();
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : <ul>
          {this.state.ratings.map(rating => <li>{JSON.stringify(rating)}</li>)}
        </ul>;

    return (
      <div>
        <h1 id="tabelLabel">Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateRatings() {
    const response = await fetch('ratings');
    const data = await response.json();
    this.setState({ ratings: data.ratings, loading: false });
  }
}
