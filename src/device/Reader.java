package device;

import java.util.ArrayList;
import java.util.Date;

import com.fazecast.jSerialComm.*;

public class Reader {

	private static SerialPort serialPort = null;

	private int readByte() {
		byte[] buf = new byte[1];
		serialPort.readBytes(buf, (long) 1);

		int var = buf[0];

		if (var < 0) {
			var = 256 - Math.abs(var); // epic java byte hack
		}
		return var;
	}

	private boolean validateChecksum(int[] data) {
		int num = 0;
		for (int index = 0; index < data.length; ++index) {
			num += data[index];
		}
		return (byte) num == (byte) 127 || (byte) num == (byte) 255;

	}

	public int[] downloadData() throws Exception {

		int lastPercentage = -1;

		ArrayList<Integer> lastRead = new ArrayList<Integer>();

		int num3 = 21;
		int num4 = num3;
		int num5 = 2;

		while (num5 < num4) {
			if ((num5 & 15) == 0) {
				int percentage = Math.min(100, num5 * 100 / num4);
				if (lastPercentage != percentage) {
					System.out.println(percentage + "%");
					lastPercentage = percentage;
				}
			}

			if (num5 == 8)
				num4 = 5 * (lastRead.get(4) + (lastRead.get(5) << 8)) + num3;

			try {
				int value = readByte();
				lastRead.add(value);
				++num5;
			} catch (Exception e) {
				throw new Exception("Error while reading data: " + e);
			}
			Thread.sleep(1);
		}

		if (serialPort.isOpen()) {
			serialPort.closePort();
		}

		ArrayList<Integer> checksumArray = new ArrayList<Integer>();
		checksumArray.add(170); // wasted these values in the while loop of the initialization???
		checksumArray.add(85); // we need them to check the checksum
		checksumArray.addAll(lastRead);

		int[] cheksumData = checksumArray.stream().mapToInt(i -> i).toArray();

		if (!validateChecksum(cheksumData)) {
			throw new Exception("Checksum error! Read data: " + checksumArray);
		}

		return lastRead.stream().mapToInt(i -> i).toArray();

	}

	private int calculateChecksum(int[] data) {
		int num = 0;
		for (int index = 0; index < data.length - 1; ++index)
			num -= data[index];
		return num;
	}

	@SuppressWarnings("deprecation")
	private void sendCurrentDate() throws Exception {

		Date now = new Date();
		int[] integerBuffer = new int[9];
		integerBuffer[0] = 170;
		integerBuffer[1] = 85;
		integerBuffer[2] = now.getSeconds();
		integerBuffer[3] = now.getMinutes();
		integerBuffer[4] = now.getHours();
		integerBuffer[5] = now.getDay();
		integerBuffer[6] = now.getMonth();
		integerBuffer[7] = (now.getYear() - 2000);
		integerBuffer[8] = 0;
		integerBuffer[8] = calculateChecksum(integerBuffer);

		byte[] byteBuffer = new byte[integerBuffer.length];
		for (int i = 0; i < integerBuffer.length; i++) {
			byteBuffer[i] = (byte) integerBuffer[i];
		}

		serialPort.writeBytes(byteBuffer, byteBuffer.length);
	}

	private void setupComPort() {

		serialPort.setBaudRate(2400);
		serialPort.setNumDataBits(8);
		serialPort.setParity(SerialPort.ODD_PARITY);
		serialPort.setNumStopBits(SerialPort.ONE_STOP_BIT);
		serialPort.setComPortTimeouts(SerialPort.TIMEOUT_READ_BLOCKING, 125, 125);
		serialPort.setParity(0);

	}

	private void initializePort() throws Exception {

		System.out.println("waiting for connection...");
		while (serialPort == null) {
			for (SerialPort sp : SerialPort.getCommPorts()) {
				if (sp.getDescriptivePortName().toLowerCase().contains("dcont")) {
					serialPort = sp;
				}
			}
			System.out.print(".");
			Thread.sleep(1000);
		}

		ArrayList<Integer> lastRead = new ArrayList<Integer>();
		int num1 = 0;
		int num2 = 0;
		boolean needInicializePort = true;

		while (!(num1 == 85 && num2 == 170)) {
			Thread.sleep(1);

			num2 = num1;
			try {
				if (needInicializePort) {

					if (serialPort.isOpen()) {
						serialPort.closePort();
					}

					setupComPort();

					System.out.println("Opening port...");

					serialPort.openPort();
					needInicializePort = false;
					serialPort.flushIOBuffers();

					Thread.sleep(1500);
					sendCurrentDate();

				}

				if (serialPort.isOpen()) {
					num1 = readByte();

				}
			}

			catch (Exception e) {
				throw e;
			}
		}
	}

	public Reader() {
		try {
			initializePort();
		//	downloadData();
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}
}
