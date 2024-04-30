//import React, { useEffect } from 'react';

declare global {
  interface Window {
    google: any;
    initMap: () => void;
  }
}

const GoogleMapsAutocomplete: React.FC = () => {
  //useEffect(() => {
  //  const loadGoogleMapsAPI = () => {
  //    if (!window.google) {
  //      const script = document.createElement('script');
  //      script.src = `https://maps.googleapis.com/maps/api/js?key=AIzaSyBekkpy5L1p1HJeD-v5i5ZUnuzFk178fZI&libraries=places&v=weekly&callback=initMap`;
  //      script.defer = true;
  //      document.head.appendChild(script);
  //    } else {
  //      initMap();
  //    }
  //  };

  //  loadGoogleMapsAPI();
  //}, []);

  //// Define initMap globally
  //window.initMap = () => {
  //  const map = new window.google.maps.Map(document.getElementById("map"), {
  //    center: { lat: 40.749933, lng: -73.98633 },
  //    zoom: 13,
  //    mapTypeControl: false,
  //  });

  //  const input = document.getElementById("pac-input") as HTMLInputElement;
  //  const autocomplete = new window.google.maps.places.Autocomplete(input, {
  //    fields: ["formatted_address", "geometry", "name"],
  //    strictBounds: false,
  //  });

  //  autocomplete.bindTo("bounds", map);

  //  const infowindow = new window.google.maps.InfoWindow();
  //  const marker = new window.google.maps.Marker({
  //    map,
  //    anchorPoint: new window.google.maps.Point(0, -29),
  //  });

  //  autocomplete.addListener("place_changed", () => {
  //    infowindow.close();
  //    marker.setVisible(false);

  //    const place = autocomplete.getPlace();

  //    if (!place.geometry || !place.geometry.location) {
  //      window.alert("No details available for input: '" + place.name + "'");
  //      return;
  //    }

  //    if (place.geometry.viewport) {
  //      map.fitBounds(place.geometry.viewport);
  //    } else {
  //      map.setCenter(place.geometry.location);
  //      map.setZoom(17);
  //    }

  //    marker.setPosition(place.geometry.location);
  //    marker.setVisible(true);
  //    infowindow.setContent(`<div><strong>${place.name}</strong><br>${place.formatted_address}</div>`);
  //    infowindow.open(map, marker);
  //  });
  //};

  return (
    <div>
      <div id="pac-container">
        <input id="pac-input" type="text" placeholder="Enter a location" />
      </div>
      <div id="map" style={{ width: '100%', height: '400px' }}></div>
    </div>
  );
};

export default GoogleMapsAutocomplete;
