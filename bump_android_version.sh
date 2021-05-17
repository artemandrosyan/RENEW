for ARGUMENT in "$@"
do

    KEY=$(echo $ARGUMENT | cut -f1 -d=)
    VALUE=$(echo $ARGUMENT | cut -f2 -d=)

    case "$KEY" in
            buildGradleFilePath)              buildGradleFilePath=${VALUE} ;;
            newVersionCode)    newVersionCode=${VALUE} ;;
            newVersionName)    newVersionName=${VALUE} ;;
            *)
    esac


done

# Check $buildGradleFilePath
if [ -z "$buildGradleFilePath" ]
then
      echo "Error: $buildGradleFilePath is NULL"
      exit 1
fi

# Check $newVersionCode
if [ -z "$newVersionCode" ]
then
      echo "newVersionCode is NULL - skipping"
else
      # For macOS: -E
      # For Unix: -r
      sed -i -r "s/(^(\s)*versionCode:?(\s)*)(.*)/\1$newVersionCode/" "$buildGradleFilePath"
      echo "newVersionCode: $newVersionCode"
fi

# Check $newVersionName
if [ -z "$newVersionName" ]
then
      echo "newVersionName is NULL - skipping"
else
      # For macOS: -E
      # For Unix: -r
      sed -i -r "s/(^(\s)*versionName:?(\s)*)(.*)/\1$newVersionName/" "$buildGradleFilePath"
      echo "newVersionName: $newVersionName"
fi
