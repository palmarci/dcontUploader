package data;

import java.time.LocalDateTime;
import java.time.ZoneId;
import java.util.Date;

public class Measurement  implements Comparable<Measurement> {
    private int measurmentIdField;
    private float valueField;
    private LocalDateTime dateField;
    private boolean isDcontField;
    private boolean markerField;
    private boolean isBloodPlasmaField;
    private int isLoHiField;
    private LocalDateTime dcontDateField;
    private boolean isVisibleField;
    private boolean isHypoField;
    private boolean isSportField;
    private boolean isPostmealField;
    private boolean isPremealField;
    private boolean isFastingField;

    public int getMeasurmentId() {
        return measurmentIdField;
    }

    public void setMeasurmentId(int measurmentIdField) {
        this.measurmentIdField = measurmentIdField;
    }

    public float getValue() {
        return valueField;
    }

    public void setValue(float valueField) {
        this.valueField = valueField;
    }

    public LocalDateTime getDate() {
        return dateField;
    }

    public void setDate(LocalDateTime dateField) {
        this.dateField = dateField;
    }

    public boolean isDcont() {
        return isDcontField;
    }

    public void setDcont(boolean dcont) {
        isDcontField = dcont;
    }

    public boolean isMarker() {
        return markerField;
    }

    public void setMarker(boolean marker) {
        this.markerField = marker;
    }

    public boolean isBloodPlasma() {
        return isBloodPlasmaField;
    }

    public void setBloodPlasma(boolean bloodPlasma) {
        isBloodPlasmaField = bloodPlasma;
    }

    public int getIsLoHi() {
        return isLoHiField;
    }

    public void setIsLoHi(int isLoHi) {
        this.isLoHiField = isLoHi;
    }

    public LocalDateTime getDcontDate() {
        return dcontDateField;
    }

    public void setDcontDate(LocalDateTime dcontDate) {
        this.dcontDateField = dcontDate;
    }

    public boolean isVisible() {
        return isVisibleField;
    }

    public void setVisible(boolean visible) {
        isVisibleField = visible;
    }

    public boolean getHypo() {
        return isHypoField;
    }

    public void setHypo(boolean hypo) {
        isHypoField = hypo;
    }

    public boolean getSport() {
        return isSportField;
    }

    public void setSport(boolean sport) {
        isSportField = sport;
    }

    public boolean getPostmeal() {
        return isPostmealField;
    }

    public void setPostmeal(boolean postmeal) {
        isPostmealField = postmeal;
    }

    public boolean getPremeal() {
        return isPremealField;
    }

    public void setPremeal(boolean premeal) {
        isPremealField = premeal;
    }

    public boolean getFasting() {
        return isFastingField;
    }

    public void setFasting(boolean fasting) {
        isFastingField = fasting;
    }

	@Override
	public int compareTo(Measurement o) {


		int oEpoch = (int)o.getDate().atZone(ZoneId.systemDefault()).toEpochSecond();
		int thisEpoch = (int)this.getDate().atZone(ZoneId.systemDefault()).toEpochSecond();
		

		return oEpoch - thisEpoch;
	}
}