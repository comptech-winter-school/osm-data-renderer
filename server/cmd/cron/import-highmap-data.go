package main

import (
	"flag"
	file_system "github.com/comptech-winter-school/osm-data-renderer/server/pkg/utils/file-system"
	"github.com/joho/godotenv"
	"log"
	"os"
)

func main() {
	var highmapFileName string
	var highmapDownloadBaseUrl string

	flag.StringVar(&highmapDownloadBaseUrl, "base_url", "https://srtm.csi.cgiar.org/wp-content/uploads/files/srtm_5x5/ASCII/", "base download url")
	flag.StringVar(&highmapFileName, "hm_name", "srtm_41_02.zip", "zip highmap data file name (srtm_44_01.zip for Moscow)")
	flag.Parse()

	err := godotenv.Load()
	if err != nil {
		log.Fatal("Error loading .env file")
	}

	log.Println("Start downloading...")
	err = file_system.DownloadFile(os.Getenv("HIGHMAP_PATH")+highmapFileName, highmapDownloadBaseUrl+highmapFileName)
	if err != nil {
		log.Fatal(err)
	}
	log.Println("Downloading finished")

	log.Println("Start unzipping...")
	err = file_system.Unzip(os.Getenv("HIGHMAP_PATH")+highmapFileName, os.Getenv("HIGHMAP_PATH"))
	if err != nil {
		log.Fatal(err)
	}
	log.Println("Done")

	log.Println("Deleting temp files...")
	err = os.Remove(os.Getenv("HIGHMAP_PATH") + highmapFileName)
	err = os.Remove(os.Getenv("HIGHMAP_PATH") + highmapFileName[:len(highmapFileName)-3] + "prj")
	err = os.Remove(os.Getenv("HIGHMAP_PATH") + "readme.txt")
	log.Println("Temp files deleted")
}
