package data;

import java.lang.reflect.Field;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

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
	
	private JSONObject getNewOrderedJsonObj() throws Exception {
		JSONObject js = new JSONObject();

		   try {
			      Field changeMap = js.getClass().getDeclaredField("map");
			      changeMap.setAccessible(true);
			      changeMap.set(js, new LinkedHashMap<>());
			      changeMap.setAccessible(false);
			    } catch (IllegalAccessException | NoSuchFieldException e) {
			      throw new Exception("Json object hashmap order hack has failed");
			    }
		return js;
			    
	}
	
	/*
	 	//they use a trailing .0 value when seriliaizing the json
		//im not sure if we should use this hack to do that
		 
	 private static String fixJsonValue(String inputJson) {

	        String pattern = "\"value\":\\s*(\\d+)(\\.0)?,";
	        Pattern regex = Pattern.compile(pattern);
	        Matcher matcher = regex.matcher(inputJson);
	        StringBuffer result = new StringBuffer();
	        while (matcher.find()) {
	            String replacement = "\"value\": " + matcher.group(1) + ".0,";
	            matcher.appendReplacement(result, replacement);
	        }
	        matcher.appendTail(result);

	        return result.toString();
	    }
	 
	 */
	
	public String AsJson() throws Exception {
			
		Collections.sort(measurements);
		Collections.reverse(measurements);

		JSONArray glucoseArray = new JSONArray();

		
		for(Measurement m : measurements) {
			JSONObject measureJson = getNewOrderedJsonObj();
		
			DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");
			String formattedDate = formatter.format(m.getDate());			
			
			
			//validity
			int validity = 2;
			if (m.getIsLoHi() == 1) {
				validity = 1;
			}
			if (m.getIsLoHi() == 2) {
				validity = 3;
			}
			
			
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
			
			measureJson.put("value", m.getValue());
			measureJson.put("meterDeviceMeasurementDate", formattedDate);
			measureJson.put("entryDate", formattedDate);
			measureJson.put("mealType", mealType);
			measureJson.put("resultValidity", validity);
			measureJson.put("measurementMetas", JSONObject.NULL); //TODO (or is it? :o)
			
			glucoseArray.put(measureJson);
		}
		
		JSONObject js = getNewOrderedJsonObj();
		
	
		js.put("deviceNumber", this.deviceId);
		js.put("glucoseList", glucoseArray);
		js.put("medicineIntakeList", JSONObject.NULL);
		js.put("mealList", JSONObject.NULL);
		js.put("ketoneList", JSONObject.NULL);
		js.put("deviceSettings", JSONObject.NULL);	

		return js.toString();
	}
	
	public String toString() {
		String firstDate = measurements.get(0).getDate().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm"));
		String lastDate = measurements.get(measurements.size() - 1).getDate().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm"));

		return "#" + deviceId + " (v" + softwareVersion + ") : " + measurements.size() + " measurements (" + firstDate + " - " + lastDate + ")";
		
	}
}
