package FileToBase64Encoding

import (
	"bufio"
	"encoding/base64"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v1/model"
	"log"
	"math"
	"os"
	"strconv"
	"strings"
)

type HeightMapHeaders struct {
	Cols         int
	Rows         int
	Xllcorner    float64
	Yllcorner    float64
	Cellsize     float64
	NODATA_value int
}

const (
	FIELDS_IN_HEADER = 6
)

func GetIntValueFromKeyValueString(keyValue []string) int {
	val, err := strconv.ParseInt(keyValue[len(keyValue)-1], 10, 32)
	if err != nil {
		log.Println("ERROR: " + err.Error())
	}
	return int(val)
}

func GetFloatValueFromKeyValueString(keyValue []string) float64 {
	val, err := strconv.ParseFloat(keyValue[len(keyValue)-1], 64)
	if err != nil {
		log.Println("ERROR: " + err.Error())
	}
	return val
}

func GetHeadersFromHeightMapFile(scanner *bufio.Scanner) HeightMapHeaders {
	var rows = 0
	var cols = 0
	var xllcorner = 0.0
	var yllcorner = 0.0
	var cellsize float64 = 0.0
	var NODATA_value = 0
	var readFields = 0
	for readFields < FIELDS_IN_HEADER {
		scanner.Scan()
		key_value := strings.Split(scanner.Text(), " ")
		if key_value[0] == "ncols" {
			cols = GetIntValueFromKeyValueString(key_value)
		} else if key_value[0] == "nrows" {
			rows = GetIntValueFromKeyValueString(key_value)
		} else if key_value[0] == "xllcorner" {
			xllcorner = GetFloatValueFromKeyValueString(key_value)
		} else if key_value[0] == "yllcorner" {
			yllcorner = GetFloatValueFromKeyValueString(key_value)
		} else if key_value[0] == "NODATA_value" {
			NODATA_value = GetIntValueFromKeyValueString(key_value)
		} else if key_value[0] == "cellsize" {
			cellsize = GetFloatValueFromKeyValueString(key_value)
		}
		readFields++
	}
	return HeightMapHeaders{cols, rows, xllcorner, yllcorner, cellsize, NODATA_value}
}

func GetSliceOfFile(scanner *bufio.Scanner, headers HeightMapHeaders, border model.Border) ([]int16, model.HeightMapSize) { //Значения в файле представлены в виде 16 битных чтсел
	startCol := int((border.XMin - headers.Xllcorner) / headers.Cellsize)
	endCol := startCol + int(math.Ceil((border.XMax-border.XMin)/headers.Cellsize))
	startRow := int((border.YMin - headers.Yllcorner) / headers.Cellsize)
	endRow := startRow + int(math.Ceil((border.YMax-border.YMin)/headers.Cellsize))
	data := make([]int16, (endRow-startRow)*(endCol-startCol))
	idx := 0
	i := 0
	for ; i < startRow; i++ { //пропускаем ненужные строки
		scanner.Scan()
	}

	for ; i < endRow; i++ {
		nums := strings.Split(scanner.Text(), " ")
		for j := startCol; j < endCol; j++ {
			parsedNum, err := strconv.ParseInt(nums[j], 10, 16)
			if err != nil {
				return nil, model.HeightMapSize{0, 0}
			}
			data[idx] = int16(parsedNum)
			idx++
		}
	}

	return data, model.HeightMapSize{endRow - startRow, endCol - startCol}
}

func GetEncodedSliceOfFile(filepath string, border model.Border) ([]byte, model.HeightMapSize, error) {
	var encodedData []byte
	var err error

	file, err := os.Open(filepath)
	if err != nil {
		return nil, model.HeightMapSize{0, 0}, err
	}
	scanner := bufio.NewScanner(file)
	headers := GetHeadersFromHeightMapFile(scanner)

	data, sizes := GetSliceOfFile(scanner, headers, border)

	var byteData = make([]byte, len(data)*2)
	for idx, int16data := range data {
		byteData[2*idx] = byte(int16data >> 8)
		byteData[2*idx+1] = byte(int16data & 0xFF)
	}

	encodedData = make([]byte, base64.StdEncoding.EncodedLen(len(byteData)))
	base64.StdEncoding.Encode(encodedData, byteData)

	//for test only
	//decodedData := make([]byte, base64.StdEncoding.DecodedLen(len(encodedData)))
	//base64.StdEncoding.Decode(decodedData, encodedData)

	return encodedData, sizes, err
}
