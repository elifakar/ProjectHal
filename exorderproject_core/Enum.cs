using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core
{
    public enum ApiResponseCode
    {
        TOKEN_TIMEOUT = 100,
        MISSING_TOKEN = 101,
        WRONG_TOKEN = 102,
        MISSING_BODY_PARAMS = 103,
        WRONG_BODY_PARAMS = 104,
        MISSING_QUERY_PARAMS = 105,
        WRONG_QUERY_PARAMS = 106,
        INSERT_ERROR = 120,
        UPDATE_ERROR = 121,
        DELETE_ERROR = 122,
        NOT_EXIST = 130,
        CONTINUE_TOKEN = 131,
        PRIMARY_FIELD = 132,
        NOT_EQUAL = 133,
        WRONG_DATA = 134,
        UNAUTHORIZED = 140,
        SUCCESS = 200,
        SERVIS_EXCEPTION = 500,
        UNABLE_TO_CONNECT = 999,
    }
    public enum UpdateCodes
    {
        URUN_GENEL = 1,
        URUN_TEDARIKCI = 2,
        KATEGORI_GENEL=3
    }
    public enum LogCodes
    {
        INSERT = 1,
        UPDATE = 2,
        DELETE = 3,
        SELECT = 4
    }
    public enum PAKETLER
    {
        HalProject = 2,
        ExorderCafe = 3,
        ETicaret = 4
    }
}
