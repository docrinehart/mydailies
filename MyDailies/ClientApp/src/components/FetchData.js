import React, { Component } from 'react';

const RatingDay = ({ props }) => (
  <div>
    <h3>{props.ratingDate}</h3>
  </div>
);

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { rating: { date: '', metrics: [] }, loading: true };
  }

  componentDidMount() {
    this.populateRatings();
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : <div>
          <h3>{this.state.rating.date}</h3>
          <ul>
            {this.state.rating.metrics.map(rating => <li>{JSON.stringify(rating)}</li>)}
          </ul>
        </div>;

    return (
      <div>
        <h1 id="tabelLabel">Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateRatings() {
    const response = await fetch('ratings?date=2019-09-29');
    const data = await response.json();
    this.setState({ rating: data, loading: false });
  }
}
