package data;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;

import org.json.JSONArray;
import org.json.JSONObject;



public class Transaction {

	private int softwareVersion;
	private String deviceId;
	private ArrayList<Measurement> measurements;
	
	public Transaction(int softwareVersion, String deviceId, ArrayList<Measurement> measurements) {
		this.setSoftwareVersion(softwareVersion);
		this.setDeviceId(deviceId);
		this.setMeasurements(measurements);
	}
	
	
	public Transaction() {
		
	}

	public int getSoftwareVersion() {
		return softwareVersion;
	}

	public void setSoftwareVersion(int softwareVersion) {
		this.softwareVersion = softwareVersion;
	}

	public String getDeviceId() {
		return deviceId;
	}

	public void setDeviceId(String deviceId) {
		this.deviceId = deviceId;
	}

	public ArrayList<Measurement> getMeasurements() {
		return measurements;
	}

	public void setMeasurements(ArrayList<Measurement> measurements) {
		this.measurements = measurements;
	}
	
	public String AsJson() {
		
		JSONObject js = new JSONObject();
		js.put("deviceNumber", this.deviceId);
		
		js.put("deviceSettings", JSONObject.NULL);
		js.put("ketoneList", JSONObject.NULL);
		js.put("mealList", JSONObject.NULL);
		js.put("medicineIntakeList", JSONObject.NULL);
		
		JSONArray glucoseArray = new JSONArray();
		
		for(Measurement m : measurements) {
			JSONObject measureJson = new JSONObject();
		
			DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-Mm-dd'T'hh:mm:ss");
			String formattedDate = formatter.format(m.getDate());			
			
			measureJson.put("entryDate", formattedDate);
			measureJson.put("meterDeviceMeasurementDate", formattedDate);
			measureJson.put("value", m.getValue());
			
			
			//validity
			int validity = 2;
			if (m.getIsLoHi() == 1) {
				validity = 1;
			}
			if (m.getIsLoHi() == 2) {
				validity = 3;
			}
			measureJson.put("resultValidity", validity);
			
			
			//mealtype
			int mealType = 5;
			
			if (m.getFasting()) {
				mealType = 1;
			}
			
			if (m.getPremeal()) {
				mealType = 2;
			}
			
			if (m.getPostmeal()) {
				mealType = 3;
			}
			measureJson.put("mealType", mealType);
			
			//metas TODO!
			measureJson.put("measurementMetas", JSONObject.NULL);
			
			
			glucoseArray.put(measureJson);
		}
		
		
		js.put("glucoseList", glucoseArray);


		return js.toString();
	}
	
	public String toString() {
		String firstDate = measurements.get(0).getDate().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm"));
		String lastDate = measurements.get(measurements.size() - 1).getDate().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm"));

		return "#" + deviceId + " (v" + softwareVersion + ") : " + measurements.size() + " measurements (" + firstDate + " - " + lastDate + ")";
		
	}
}
