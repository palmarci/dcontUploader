package device;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Date;

import data.Measurement;
import data.Transaction;

public class Parser {

	private Transaction transaction = null;
	private static int[] rawData = null;

	public Parser(int[] data) throws Exception {

		if (data == null || data.length == 0) {
			throw new Exception("Device data is null!");
		}

		this.rawData = data;
		this.transaction = new Transaction();
		parseInformation();
		extractMeasurements();

	}

	public Transaction getTransaction() {
		return this.transaction;
	}

	private void parseInformation() {
		int dcontId = ((int) rawData[rawData.length - 5] | (int) rawData[rawData.length - 4] << 8
				| (int) rawData[rawData.length - 3] << 16 | (int) rawData[rawData.length - 2] << 24);
		this.transaction.setDeviceId(Integer.toString(dcontId));
		int version = rawData[rawData.length - 9];

		this.transaction.setSoftwareVersion(version);
	}

	private void extractMeasurements() throws Exception {
		float low = 0.6f;
		float high = 33.3f;
		int fieldSize = 5;

		try {
			int num = (rawData.length - 19) / fieldSize;
			if (num == 0) {
				throw new Exception("No data to extract!");
			}
			ArrayList<Measurement> measurements = new ArrayList<Measurement>();

			int year = LocalDate.now().getYear();
			int month1 = LocalDate.now().getMonthValue();

			for (int index1 = 0; index1 < num; ++index1) {
				Measurement measurement = new Measurement();
				int index2 = index1 * fieldSize + 10;
				measurement.setValue(
						(float) (((double) rawData[index2] + (double) (((int) rawData[index2 + 1] & 1) << 8)) / 10.0));
				if ((double) measurement.getValue() <= (double) low && (double) measurement.getValue() != 0.0) {
					measurement.setValue(0.1f);
					measurement.setIsLoHi(1);
				}
				if ((double) measurement.getValue() >= (double) high) {
					measurement.setValue(55.4f);
					measurement.setIsLoHi(2);
				}
				int month2 = ((int) rawData[index2 + 1] >> 4) + 1;
				int day = ((int) rawData[index2 + 3] >> 3) + 1;
				int hour = (int) rawData[index2 + 2] >> 6 & 3 | ((int) rawData[index2 + 3] & 7) << 2;
				int minute = (int) rawData[index2 + 2] & 63;
				
				measurement.setMarker(((int) rawData[index2 + 1] & 2) == 2);
				measurement.setBloodPlasma(((int) rawData[index2 + 1] & 4) == 4);
				
				if (this.transaction.getSoftwareVersion() >= 96) {
					if (this.transaction.getSoftwareVersion() >= 100) {
						// boolean()
						measurement.setHypo(((int) rawData[index2 + 1] & 12) >> 2 == 2);
						if (this.transaction.getSoftwareVersion() == 103) {
							measurement.setHypo(((int) rawData[index2 + 1] & 8) == 8);
							measurement.setSport((int) rawData[index2 + 4] >> 7 == 1);
							measurement.setPostmeal(((int) rawData[index2 + 4] >> 6 & 1) == 1);
							measurement.setPremeal(((int) rawData[index2 + 4] >> 5 & 1) == 1);
						} else {
							switch (((int) rawData[index2 + 4] & 224) >> 5) {
							case 1:
								measurement.setPremeal(true);
								break;
							case 2:
								measurement.setPostmeal(true);
								break;
							case 3:
								measurement.setSport(true);
								break;
							case 4:
								measurement.setFasting(true);
								break;
							case 5:
								measurement.setMarker(true);
								break;
							}
						}
					} else {
						measurement.setHypo(((int) rawData[index2 + 1] & 8) == 8);
						measurement.setSport((int) rawData[index2 + 4] >> 7 == 1);
						measurement.setPostmeal(((int) rawData[index2 + 4] >> 6 & 1) == 1);
						measurement.setPremeal(((int) rawData[index2 + 4] >> 5 & 1) == 1);
					}
				}
				// if (fieldSize == 5) {
				year = 2008 + ((int) rawData[index2 + 4] & 31);
				// }
	
				
		


				
			      LocalDate date = LocalDate.of(year, month2, day);
			        LocalTime time = LocalTime.of(hour, minute, 0);
			        LocalDateTime localDateTime = LocalDateTime.of(date, time);
			      //  DateTimeFormatter format = DateTimeFormatter.ofPattern("MMM d yyyy  hh:mm a");
			//        System.out.println(localDateTime.format(format));
			        
				
				
				measurement.setDate(localDateTime);
				measurement.setDcontDate(localDateTime);
				
				
				measurement.setVisible(true);

				if ((double) measurement.getValue() != 0.0) {
					measurements.add(measurement);
				}
			}

			// measurements reverse todo
	        Collections.sort(measurements);
	        
	//        return measurements;


			this.transaction.setMeasurements(measurements);
			// result.setData(measurementRecordList);
		} catch (Exception ex) {
			// result.error = ex.ToString();
			throw ex;
		}

	}

}
