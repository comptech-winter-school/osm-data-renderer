package file_system

import (
	"io/ioutil"
	"os"
)

func Copy(src, dest string) error {
	bytesRead, err := ioutil.ReadFile(src)

	if err != nil {
		return err
	}

	err = ioutil.WriteFile(dest, bytesRead, os.ModePerm)

	if err != nil {
		return err
	}
	return nil
}
