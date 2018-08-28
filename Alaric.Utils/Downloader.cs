using System;
using System.IO;
using System.Net;

namespace Alaric.Utils
{
    public static class Downloader
    {
        /// <summary>
        /// Download file from a url.
        /// </summary>
        public static bool Download(string url, string localFile)
        {
            int a = 0;
            return Download(url, localFile, ref a);
        }

        /// <summary>
        /// Download file from a url.
        /// </summary>
        public static bool Download(string url, string localFile, ref int downloadPercent)
        {
            bool flag;
            FileStream writeStream;

            long remoteFileLength = GetHttpLength(url);
            if (remoteFileLength==745)
            {
                return false;
            }

            long startPosition;

            if (File.Exists(localFile))
            {

                writeStream = File.OpenWrite(localFile);
                startPosition = writeStream.Length;

                if (startPosition >= remoteFileLength)
                {
                    writeStream.Close();
                    return false;
                }
                writeStream.Seek(startPosition, SeekOrigin.Current);
            }
            else
            {
                writeStream = new FileStream(localFile, FileMode.Create);
                startPosition = 0;
            }


            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);

                if (startPosition > 0)
                    myRequest.AddRange((int) startPosition);

                Stream readStream = myRequest.GetResponse().GetResponseStream();

                byte[] btArray = new byte[512];
                if (readStream != null)
                {
                    int contentSize = readStream.Read(btArray, 0, btArray.Length);

                    long currPosition = startPosition;
                    while (contentSize > 0)
                    {
                        currPosition += contentSize;
                        downloadPercent = (int)(currPosition*100/ remoteFileLength);
                        writeStream.Write(btArray, 0, contentSize);
                        contentSize = readStream.Read(btArray, 0, btArray.Length);
                    }
                }

                writeStream.Close();
                readStream?.Close();
                flag = true;
            }
            catch (Exception)
            {
                writeStream.Close();
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// Returns the length of the file that the specified url represented.
        /// </summary>
        public static long GetHttpLength(string url)
        {
            long length=0;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
                if (rsp.StatusCode == HttpStatusCode.OK)
                    length = rsp.ContentLength;
                rsp.Close();
                return length;
            }
            catch(Exception){
                return length;
            }         
        }
    }
}