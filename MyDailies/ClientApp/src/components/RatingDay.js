import React, { useState, useEffect } from 'react';

const convertDate = (date) => date.toISOString().slice(0, 10);

const RatingWrapper = () => {
  const today = convertDate(new Date(Date.now()));
  const [inputRatingDate, setInputRatingDate] = useState(today);
  const [ratingDate, setRatingDate] = useState(today);

  const handleInput = (e) => setInputRatingDate(e.target.value);

  const resetToday = () => {
    setInputRatingDate(today);
    setRatingDate(today);
  }

  const addDay = () => { 
    const date = new Date(inputRatingDate);
    date.setDate(date.getDate() + 1);
    const newDate = convertDate(date);
    setInputRatingDate(newDate);
    setRatingDate(newDate);
  }

  const minusDay = () => { 
    const date = new Date(inputRatingDate);
    date.setDate(date.getDate() - 1);
    const newDate = convertDate(date);
    setInputRatingDate(newDate);
    setRatingDate(newDate);
  }

  return (
    <div>
      <input type="text" value={inputRatingDate} onChange={handleInput}></input>
      <button onClick={() => setRatingDate(inputRatingDate)}>Update</button>
      <button onClick={() => minusDay()}>-</button>
      <button onClick={() => resetToday()}>Today</button>
      <button onClick={() => addDay()}>+</button>
      <hr/>
      <RatingDay ratingDate={ratingDate} />
    </div>
  );
};

const RatingDay = ({ ...props }) => {
  const [rating, setRating] = useState({ date: '', metrics: [] });

  const populateRatings = async () => {
    const response = await fetch(`rating?date=${props.ratingDate}`);
    const data = await response.json();
    setRating(data);
  }

  useEffect(() => {
    populateRatings();
  }, [props.ratingDate]);

  return (
    <div>
      <h3>{rating && rating.date}</h3>
      <table>
        <thead>
          <th>Metric</th>
          <th>Rating</th>
          <th colspan="2">Notes</th>
        </thead>
        <tbody>
          {rating && rating.metrics.map(m => (
            <tr key={m.metric}>
              <td style={{ whiteSpace: 'nowrap' }}>{m.metric}</td>
              <td>{m.rating}</td>
              <td colspan="2">{m.notes}</td>
            </tr>
          ))}
        </tbody>
      </table>
      <hr/>
      <RatingForm ratingDate={rating.date} setRating={setRating} />
    </div>
  );
};

const RatingForm = ({ ...props }) => {
  const [metric, setMetric] = useState(1);
  const [rating, setRating] = useState(10);
  const [notes, setNotes] = useState('');

  const handleMetricChange = e => setMetric(parseInt(e.target.value));
  const handleRatingChange = e => setRating(parseInt(e.target.value));
  const handleNotesChange = e => setNotes(e.target.value);

  const handleFormSubmit = async (e, a, b, c) => {    
    e.preventDefault();

    const formData = {
      date: props.ratingDate,
      metricId: metric,
      rating,
      notes
    };

    var response = await fetch('rating/save', {
      method: 'post',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(formData)
    })

    var responseData = await response.json();
    props.setRating(responseData.newDay);

    setRating(10);
    setNotes('');
  };
  
  return (
    <form onSubmit={handleFormSubmit}>
      <label htmlFor="metricId">Metric (1-work, 2-home, 3-exercise)</label><br/>
      <input name="metricId" type="number" min="1" max="3" value={metric} onChange={handleMetricChange} /><br/>
      
      <label htmlFor="ratingValue">Rating</label><br/>
      <input name="ratingValue" type="number" min="0" max="10" value={rating} onChange={handleRatingChange}></input><br/>

      <label htmlFor="notes">Notes</label><br/>
      <textarea name="notes" value={notes} onChange={handleNotesChange}></textarea><br/>

      <button type="submit">Add Rating</button>
    </form>
  );
};

RatingDay.displayName = RatingDay.name;

export default RatingWrapper;