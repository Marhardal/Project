var simplemaps_countrymap_mapdata={
  main_settings: {
   //General settings
    width: "responsive", //'700' or 'responsive'
    background_color: "#FFFFFF",
    background_transparent: "yes",
    border_color: "#ffffff",
    
    //State defaults
    state_description: "State description",
    state_color: "#88A4BC",
    state_hover_color: "#3B729F",
    state_url: "",
    border_size: 1.5,
    all_states_inactive: "no",
    all_states_zoomable: "yes",
    
    //Location defaults
    location_description: "Location description",
    location_url: "",
    location_color: "#FF0067",
    location_opacity: 0.8,
    location_hover_opacity: 1,
    location_size: 25,
    location_type: "square",
    location_image_source: "frog.png",
    location_border_color: "#FFFFFF",
    location_border: 2,
    location_hover_border: 2.5,
    all_locations_inactive: "no",
    all_locations_hidden: "no",
    
    //Label defaults
    label_color: "#ffffff",
    label_hover_color: "#ffffff",
    label_size: 16,
    label_font: "Arial",
    label_display: "auto",
    label_scale: "yes",
    hide_labels: "no",
    hide_eastern_labels: "no",
   
    //Zoom settings
    zoom: "yes",
    manual_zoom: "yes",
    back_image: "no",
    initial_back: "no",
    initial_zoom: "-1",
    initial_zoom_solo: "no",
    region_opacity: 1,
    region_hover_opacity: 0.6,
    zoom_out_incrementally: "yes",
    zoom_percentage: 0.99,
    zoom_time: 0.5,
    
    //Popup settings
    popup_color: "white",
    popup_opacity: 0.9,
    popup_shadow: 1,
    popup_corners: 5,
    popup_font: "12px/1.5 Verdana, Arial, Helvetica, sans-serif",
    popup_nocss: "no",
    
    //Advanced settings
    div: "map",
    auto_load: "yes",
    url_new_tab: "no",
    images_directory: "default",
    fade_time: 0.1,
    link_text: "View Website",
    popups: "detect",
    state_image_url: "",
    state_image_position: "",
    location_image_url: ""
  },
  state_specific: {
    MWBA: {
      color: "#00d396",
      name: "Balaka"
    },
    MWBL: {
      color: "#07ffb8",
      name: "Blantyre"
    },
    MWCK: {
      color: "#07ffb8",
      name: "Chikwawa"
    },
    MWCR: {
      name: "Chiradzulu",
      description: " "
    },
    MWCT: {
      name: "Chitipa",
      description: " "
    },
    MWDE: {
      name: "Dedza",
      description: " "
    },
    MWDO: {
      name: "Dowa",
      description: " "
    },
    MWKR: {
      color: "#006d4e",
      name: "Karonga",
      description: " "
    },
    MWKS: {
      name: "Kasungu",
      description: " "
    },
    MWLI: {
      color: "#003a29",
      name: "Lilongwe",
      description: " "
    },
    MWLK: {
      name: "Likoma",
      description: " "
    },
    MWMC: {
      name: "Mchinji",
      description: " "
    },
    MWMG: {
      name: "Mangochi",
      description: " "
    },
    MWMH: {
      name: "Machinga",
      description: " "
    },
    MWMU: {
      name: "Mulanje",
      description: " "
    },
    MWMW: {
      name: "Mwanza",
      description: " "
    },
    MWMZ: {
      name: "Mzimba",
      description: " "
    },
    MWNB: {
      color: "#00d396",
      name: "Nkhata Bay",
      description: " "
    },
    MWNE: {
      name: "Neno",
      description: " "
    },
    MWNI: {
      name: "Ntchisi",
      description: " "
    },
    MWNK: {
      name: "Nkhotakota",
      description: " "
    },
    MWNS: {
      name: "Nsanje",
      description: " "
    },
    MWNU: {
      name: "Ntcheu",
      description: " "
    },
    MWPH: {
      name: "Phalombe",
      description: " "
    },
    MWRU: {
      name: "Rumphi",
      description: " "
    },
    MWSA: {
      name: "Salima",
      description: " "
    },
    MWTH: {
      name: "Thyolo",
      description: " "
    },
    MWZO: {
      name: "Zomba",
      description: " "
    }
  },
  locations: {
    "0": {
      name: "Lilongwe",
      lat: "-13.966919",
      lng: "33.787247"
    }
  },
  labels: {
    MWBA: {
      name: "Balaka",
      parent_id: "MWBA"
    },
    MWBL: {
      name: "Blantyre",
      parent_id: "MWBL"
    },
    MWCK: {
      name: "Chikwawa",
      parent_id: "MWCK"
    },
    MWCR: {
      name: "Chiradzulu",
      parent_id: "MWCR"
    },
    MWCT: {
      name: "Chitipa",
      parent_id: "MWCT"
    },
    MWDE: {
      name: "Dedza",
      parent_id: "MWDE"
    },
    MWDO: {
      name: "Dowa",
      parent_id: "MWDO"
    },
    MWKR: {
      name: "Karonga",
      parent_id: "MWKR"
    },
    MWKS: {
      name: "Kasungu",
      parent_id: "MWKS"
    },
    MWLI: {
      name: "Lilongwe",
      parent_id: "MWLI"
    },
    MWLK: {
      name: "Likoma",
      parent_id: "MWLK"
    },
    MWMC: {
      name: "Mchinji",
      parent_id: "MWMC"
    },
    MWMG: {
      name: "Mangochi",
      parent_id: "MWMG"
    },
    MWMH: {
      name: "Machinga",
      parent_id: "MWMH"
    },
    MWMU: {
      name: "Mulanje",
      parent_id: "MWMU"
    },
    MWMW: {
      name: "Mwanza",
      parent_id: "MWMW"
    },
    MWMZ: {
      name: "Mzimba",
      parent_id: "MWMZ"
    },
    MWNB: {
      name: "Nkhata Bay",
      parent_id: "MWNB"
    },
    MWNE: {
      name: "Neno",
      parent_id: "MWNE"
    },
    MWNI: {
      name: "Ntchisi",
      parent_id: "MWNI"
    },
    MWNK: {
      name: "Nkhotakota",
      parent_id: "MWNK"
    },
    MWNS: {
      name: "Nsanje",
      parent_id: "MWNS"
    },
    MWNU: {
      name: "Ntcheu",
      parent_id: "MWNU"
    },
    MWPH: {
      name: "Phalombe",
      parent_id: "MWPH"
    },
    MWRU: {
      name: "Rumphi",
      parent_id: "MWRU"
    },
    MWSA: {
      name: "Salima",
      parent_id: "MWSA"
    },
    MWTH: {
      name: "Thyolo",
      parent_id: "MWTH"
    },
    MWZO: {
      name: "Zomba",
      parent_id: "MWZO"
    }
  },
  legend: {
    entries: []
  },
  regions: {},
  data: {
    data: {
      MWBA: "20",
      MWBL: "2",
      MWCK: "6",
      MWLI: "50",
      MWNB: "12",
      MWKR: "32"
    }
  }
};