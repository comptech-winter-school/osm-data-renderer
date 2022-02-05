package main

import (
	"flag"
	filesystem "github.com/comptech-winter-school/osm-data-renderer/server/pkg/utils/file-system"
	"github.com/joho/godotenv"
	"log"
	"os"
)

const (
	heightmapBaseUrlArgName         = "base_url"
	heightmapDefaultDownloadBaseUrl = "https://srtm.csi.cgiar.org/wp-content/uploads/files/srtm_5x5/ASCII/"
	heightmapBaseUrlUsage           = "base download url"

	heightmapFilenameArgName = "hm_name"
	heightmapDefaultFileName = "srtm_41_02.zip"
	heightmapFileNameUsage   = "zip heightmap data file name (srtm_44_01.zip for Moscow)"
)

func main() {
	var heightmapFileName string
	var heightmapDownloadBaseUrl string

	flag.StringVar(&heightmapDownloadBaseUrl, heightmapBaseUrlArgName, heightmapDefaultDownloadBaseUrl, heightmapBaseUrlUsage)
	flag.StringVar(&heightmapFileName, heightmapFilenameArgName, heightmapDefaultFileName, heightmapFileNameUsage)
	flag.Parse()

	err := godotenv.Load()
	if err != nil {
		log.Fatal("Error loading .env file")
	}

	log.Println("Start downloading...")
	err = filesystem.DownloadFile(os.Getenv("HEIGHTMAPS_PATH")+heightmapFileName, heightmapDownloadBaseUrl+heightmapFileName)
	if err != nil {
		log.Fatal(err)
	}
	log.Println("Downloading finished")

	log.Println("Start unzipping...")
	err = filesystem.Unzip(os.Getenv("HEIGHTMAPS_PATH")+heightmapFileName, os.Getenv("HEIGHTMAPS_PATH"))
	if err != nil {
		log.Fatal(err)
	}
	log.Println("Done")

	log.Println("Deleting temp files...")
	err = filesystem.Copy(os.Getenv("HEIGHTMAPS_PATH")+heightmapFileName[:len(heightmapFileName)-3]+"asc",
		os.Getenv("HEIGHTMAPS_PATH")+os.Getenv("LATEST_HEIGHTMAP_NAME"))
	err = os.Remove(os.Getenv("HEIGHTMAPS_PATH") + heightmapFileName)
	err = os.Remove(os.Getenv("HEIGHTMAPS_PATH") + heightmapFileName[:len(heightmapFileName)-3] + "prj")
	err = os.Remove(os.Getenv("HEIGHTMAPS_PATH") + "readme.txt")
	log.Println("Temp files deleted")
}
