package upload;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;

import org.json.JSONException;
import org.json.JSONObject;

public class Uploader {

	private static String clientSecret = "e11370f8-23c9-4b68-a02d-c469d3f65369";
	private static String tokenUrl = "https://fiok.dcont.hu/connect/token";
	private static String transferUrl = "https://api.dcont.hu/v1/transfer";
	private static String uploadUrl = "https://enaplo.dcont.hu/adatfeltoltes/";

	private String token = "";

	private static String postRequest(String targetURL, String urlParameters, ArrayList<String> extraHeaders)
			throws Exception {

		HttpURLConnection conn = null;


		// Create connection
		URL url = new URL(targetURL);
		conn = (HttpURLConnection) url.openConnection();
		conn.setRequestMethod("POST");
		conn.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");

		// add headers
		if (extraHeaders != null) {
			for (String h : extraHeaders) {
				String[] header = h.split(" : ");
				conn.setRequestProperty(header[0], header[1]);
			}
		}

		//set request values
		conn.setRequestProperty("Content-Length", Integer.toString(urlParameters.getBytes().length));
		conn.setRequestProperty("Content-Language", "en-US");
		conn.setUseCaches(false);
		conn.setDoOutput(true);


		// Send request
		DataOutputStream wr = new DataOutputStream(conn.getOutputStream());
		wr.writeBytes(urlParameters);
		wr.close();

		// read response
		BufferedReader br = null;
		if (conn.getResponseCode() == 200) {
			br = new BufferedReader(new InputStreamReader(conn.getInputStream()));
			String strCurrentLine;
			ArrayList<String> lines = new ArrayList<String>();

			while ((strCurrentLine = br.readLine()) != null) {
				lines.add(strCurrentLine);
			}
			return String.join("\n", lines);
		} else {
			br = new BufferedReader(new InputStreamReader(conn.getErrorStream()));
			String strCurrentLine;
			ArrayList<String> errorLines = new ArrayList<String>();
			while ((strCurrentLine = br.readLine()) != null) {
				errorLines.add(strCurrentLine);
			}
			JSONObject resp = new JSONObject(String.join("\n", errorLines));
			JSONObject errors = (JSONObject) resp.get("errors");
			String serverError = errors.get("").toString(); 	// first item of an array hack
			serverError = serverError.replace("\"", ""); 		//cleaning hack, sorry not sorry lol
			serverError = serverError.replace("[", "");
			serverError = serverError.replace("]", "");
			throw new Exception("Server returned an error: \"" + serverError + "\"");
		}
	}

	private String getToken() throws JSONException, Exception {
		String params = "grant_type=client_credentials&client_id=dcont.desktop.win&client_secret=" + clientSecret
				+ "&audience=dcont.transfer";
		String respString = postRequest(tokenUrl, params, null);
		JSONObject resp = new JSONObject(respString);
		return resp.getString("access_token");

	}

	private String postData(String data) throws Exception {
		ArrayList<String> headers = new ArrayList<String>();
		headers.add("Content-Type : application/json");
		headers.add("Authorization : Bearer " + this.token);
		String resp = postRequest(transferUrl, "application/json=" + data, headers);
		return resp;
	}

	public Uploader(String uploadData) throws Exception {
		this.token = getToken();
		if (this.token == "") {
			throw new Exception("Token is empty");
		}
		postData(uploadData);
		//return uploadUrl + response number
	}

}
