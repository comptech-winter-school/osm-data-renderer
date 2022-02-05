package file_system

import (
	"io"
	"net/http"
	"os"
	"path/filepath"
)

func DownloadFile(path string, url string) (err error) {
	os.MkdirAll(filepath.Dir(path), os.ModePerm)

	// Create the file
	out, err := os.Create(path)
	if err != nil {
		return err
	}
	defer out.Close()

	// Get the data
	resp, err := http.Get(url)
	if err != nil {
		return err
	}
	defer resp.Body.Close()

	// Writer the body to file
	_, err = io.Copy(out, resp.Body)
	if err != nil {
		return err
	}

	return nil
}
