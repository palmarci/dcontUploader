package main;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Scanner;

import com.fazecast.jSerialComm.SerialPort;

import data.Transaction;
import device.Parser;
import device.Reader;
import upload.Uploader;

public class Main {

	private static String DUMP_FILENAME = "last_dump.txt";
	private static boolean DEBUG = true;


	public static void writeToFile(int[] data, String filename) throws IOException {
		File fout = new File(filename);
		FileOutputStream fos = new FileOutputStream(fout);

		PrintWriter bw = new PrintWriter(new OutputStreamWriter(fos));

		for (int i : data) {
			bw.write(Integer.toString(i));
			bw.write("\n");
		}

		bw.close();
	}

	private static int[] loadFromFile(String file) {
		ArrayList<Integer> arr = new ArrayList<Integer>();
		try {
			Scanner scanner = new Scanner(new File(file));

			while (scanner.hasNextLine()) {
				int num = Integer.parseInt(scanner.nextLine());
				arr.add(num);
			}

			scanner.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}

		return arr.stream().mapToInt(i -> i).toArray();
	}

	public static void main(String[] Args) throws Exception {
		
		int[] data;
		
		if (DEBUG) {
			data = loadFromFile(DUMP_FILENAME);

		} else {
			
			Reader reader = new Reader();
			data = reader.downloadData();
			writeToFile(data, DUMP_FILENAME);

		}
		

		

		Parser parser = new Parser(data);
		Transaction tr = parser.getTransaction();
		String json = tr.AsJson();
		
		
	//	Files.write( Paths.get("/tmp/lastJson.txt"), json.getBytes());
		
		Uploader apiMgr = new Uploader(json);
	

		
		System.out.println(tr);
	}

}
