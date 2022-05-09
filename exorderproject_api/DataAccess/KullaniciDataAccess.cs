using exorderproject_api.Helpers;
using exorderproject_core.DTO;
using exorderproject_core.Helpers;
using exorderproject_core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace exorderproject_api.DataAccess
{
    public class KullaniciDataAccess
    {
        SqlConnection con;
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];
        EncryptionAndDecryptionHelper EncryptionAndDecryption = new EncryptionAndDecryptionHelper();

        internal KULLANICI GetByEmail(string email)
        {
            KULLANICI kullanici = new KULLANICI();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM TKULLANICI WHERE KULLANICI_ADI = @KULLANICI_ADI";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@KULLANICI_ADI", email);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        kullanici = new KULLANICI()
                        {
                            ID = reader["KULLANICI_ID"].milToInt32(),
                            KULLANICI_ADSOYAD = reader["KULLANICI_ADSOYAD"].milToString(),
                            KULLANICI_ADI = reader["KULLANICI_ADI"].milToString(),
                            SIFRE = EncryptionAndDecryption.Decrypt(reader["KULLANICI_SIFRE"].milToString()),
                            TEL = reader["KULLANICI_TEL"].milToString(),
                            FIRMA_ID = reader["KULLANICI_FIRMA_ID"].milToInt32(),
                            DURUM_ID = reader["KULLANICI_DURUM_ID"].milToInt32()
                        };
                    }
                    else
                        kullanici = null;

                    con.Close();
                }
                return kullanici;
            }
        }

        internal KULLANICI Register(Register register)
        {
            KULLANICI kullanici = new KULLANICI();

            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"INSERT INTO TKULLANICI 
                ( KULLANICI_ADSOYAD, KULLANICI_ADI, KULLANICI_SIFRE,KULLANICI_EMAIL, KULLANICI_TEL, KULLANICI_FIRMA_ID, KULLANICI_DURUM_ID, KULLANICI_CREATED_AT ) 
                VALUES ( @KULLANICI_ADSOYAD, @KULLANICI_ADI, @KULLANICI_SIFRE,@KULLANICI_EMAIL, @KULLANICI_TEL, @KULLANICI_FIRMA_ID, @KULLANICI_DURUM_ID, @KULLANICI_CREATED_AT );  SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@KULLANICI_ADSOYAD", register.KULLANICI_ADSOYAD.milToString());
                    cmd.Parameters.AddWithValue("@KULLANICI_ADI", register.KULLANICI_ADI.milToString());
                    cmd.Parameters.AddWithValue("@KULLANICI_EMAIL", register.EMAIL.milToString());
                    cmd.Parameters.AddWithValue("@KULLANICI_SIFRE", EncryptionAndDecryption.Encrypt(register.SIFRE.milToString()));
                    cmd.Parameters.AddWithValue("@KULLANICI_TEL", register.TEL.milToString());
                    cmd.Parameters.AddWithValue("@KULLANICI_FIRMA_ID", 0);
                    cmd.Parameters.AddWithValue("@KULLANICI_DURUM_ID", 1);
                    cmd.Parameters.AddWithValue("@KULLANICI_CREATED_AT", DateTime.Now.milToDateTime());

                    kullanici.ID = cmd.ExecuteScalar().milToInt32();

                    con.Close();
                }
            }

            kullanici.KULLANICI_ADSOYAD = register.KULLANICI_ADSOYAD;
            kullanici.KULLANICI_ADI = register.KULLANICI_ADI;
            kullanici.EMAIL = register.EMAIL;
            kullanici.SIFRE = register.SIFRE;
            kullanici.TEL = register.TEL;
            kullanici.DURUM_ID = 1;
            kullanici.FIRMA_ID = 0;
            return kullanici;
        }

        internal PASSWORDRESETTEMP ForgotPassword(ForgotPassword forgotpassword, int id, string guid)
        {
            PASSWORDRESETTEMP passwordresettemp = new PASSWORDRESETTEMP();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"INSERT INTO TPASSWORDRESETTEMP 
                ( PRT_KULLANICI_ID, PRT_GUID, PRT_SONTARIH ) 
                VALUES ( @PRT_KULLANICI_ID, @PRT_GUID, @PRT_SONTARIH);  SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@PRT_KULLANICI_ID", id);
                    cmd.Parameters.AddWithValue("@PRT_GUID", guid);
                    cmd.Parameters.AddWithValue("@PRT_SONTARIH", DateTime.Now.AddHours(2).milToDateTime());
                    passwordresettemp.ID = cmd.ExecuteScalar().milToInt32();

                    con.Close();
                }
            }
            return passwordresettemp;
        }

        internal PASSWORDRESETTEMP CheckGuidAndGetTime(ResetPassword resetpassword)
        {
            PASSWORDRESETTEMP passwordresettemp = new PASSWORDRESETTEMP();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT PRT_KULLANICI_ID,PRT_SONTARIH FROM TPASSWORDRESETTEMP WHERE PRT_GUID = @PRT_GUID";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@PRT_GUID", resetpassword.GUID.milToString());
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        passwordresettemp = new PASSWORDRESETTEMP()
                        {
                            KULLANICI_ID = reader["PRT_KULLANICI_ID"].milToInt32(),
                            SONTARIH = reader["PRT_SONTARIH"].milToDateTime()
                        };
                    }
                    else
                        passwordresettemp = null;

                    con.Close();
                }
                return passwordresettemp;
            }
        }

        internal bool ChangePassword(string sifre, int id)
        {
            bool result = false;

            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"UPDATE TKULLANICI SET
                                    KULLANICI_SIFRE = @KULLANICI_SIFRE WHERE KULLANICI_ID = @KULLANICI_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@KULLANICI_SIFRE", EncryptionAndDecryption.Encrypt(sifre));
                    cmd.Parameters.AddWithValue("@KULLANICI_ID", id);

                    result = cmd.ExecuteNonQuery() > 0 ? true : false;

                    con.Close();
                }
            }
            return result;
        }

        internal bool CheckEmail(string email)
        {
            bool result = false;

            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM TKULLANICI WHERE KULLANICI_ADI = @KULLANICI_ADI";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@KULLANICI_ADI", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    result = count > 0 ? true : false;

                    con.Close();
                }
            }
            return result;
        }

        internal string GetPasswordById(int id)
        {
            using (con = new SqlConnection(connectionString))
            {
                string sifre;
                string sqlQuery = $@"SELECT KULLANICI_SIFRE FROM TKULLANICI WHERE KULLANICI_ID = @KULLANICI_ID";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@KULLANICI_ID", id);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        sifre = EncryptionAndDecryption.Decrypt(reader["KULLANICI_SIFRE"].milToString());
                    }
                    else
                        sifre = null;

                    con.Close();
                }
                return sifre;
            }
        }

        internal List<KULLANICI> GetAll(int firma, int? durum)
        {
            List<KULLANICI> kullanicilar = new List<KULLANICI>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM TKULLANICI WHERE KULLANICI_FIRMA_ID = @KULLANICI_FIRMA_ID ";
                sqlQuery += durum.milToInt32() > 0 ? " AND KULLANICI_DURUM_ID = @KULLANICI_DURUM_ID " : "";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@KULLANICI_FIRMA_ID", firma);
                    if (durum.milToInt32() > 0) cmd.Parameters.AddWithValue("@KULLANICI_DURUM_ID", durum.milToInt32());

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        kullanicilar.Add(new KULLANICI()
                        {
                            ID = reader["KULLANICI_ID"].milToInt32(),
                            KULLANICI_ADSOYAD = reader["KULLANICI_ADSOYAD"].milToString(),
                            KULLANICI_ADI = reader["KULLANICI_ADI"].milToString(),
                            EMAIL = reader["KULLANICI_EMAIL"].milToString(),
                            TEL = reader["KULLANICI_TEL"].milToString(),
                            FIRMA_ID = reader["KULLANICI_FIRMA_ID"].milToInt32(),
                            DURUM_ID = reader["KULLANICI_DURUM_ID"].milToInt32(),
                            CREATED_AT = reader["KULLANICI_CREATED_AT"].milToDateTime(),
                            CREATED_BY = reader["KULLANICI_CREATED_BY"].milToInt32(),
                            EDITED_AT = reader["KULLANICI_EDITED_AT"].milToDateTime(),
                            EDITED_BY = reader["KULLANICI_EDITED_BY"].milToInt32()

                        });
                    }

                    con.Close();
                }
                return kullanicilar;
            }
        }

        internal KULLANICI GetById(int id)
        {
            KULLANICI kullanici = new KULLANICI();
            if (id > 0)
            {
                using (con = new SqlConnection(connectionString))
                {
                    string sqlQuery = $@"SELECT * FROM TKULLANICI WHERE KULLANICI_ID = @KULLANICI_ID";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@KULLANICI_ID", id);
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            kullanici = new KULLANICI()
                            {
                                ID = reader["KULLANICI_ID"].milToInt32(),
                                KULLANICI_ADSOYAD = reader["KULLANICI_ADSOYAD"].milToString(),
                                KULLANICI_ADI = reader["KULLANICI_ADI"].milToString(),
                                EMAIL = reader["KULLANICI_EMAIL"].milToString(),
                                TEL = reader["KULLANICI_TEL"].milToString(),
                                FIRMA_ID = reader["KULLANICI_FIRMA_ID"].milToInt32(),
                                DURUM_ID = reader["KULLANICI_DURUM_ID"].milToInt32(),
                                CREATED_AT = reader["KULLANICI_CREATED_AT"].milToDateTime(),
                                CREATED_BY = reader["KULLANICI_CREATED_BY"].milToInt32(),
                                EDITED_AT = reader["KULLANICI_EDITED_AT"].milToDateTime(),
                                EDITED_BY = reader["KULLANICI_EDITED_BY"].milToInt32()
                            };
                        }
                        else
                            kullanici = null;

                        con.Close();
                    }
                }
            }
            else
                kullanici = null;

            return kullanici;
        }

        internal KULLANICI Insert(KULLANICI kullanici)
        {
            KULLANICI insertedKullanici = new KULLANICI();
            int id;

            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"INSERT INTO TKULLANICI 
                ( KULLANICI_ADI, KULLANICI_SOYADI, KULLANICI_ADI, KULLANICI_SIFRE,KULLANICI_EMAIL, KULLANICI_TEL, KULLANICI_FIRMA_ID, KULLANICI_DURUM_ID,KULLANICI_CREATED_AT, KULLANICI_CREATED_BY ) 
                VALUES ( @KULLANICI_ADI, @KULLANICI_SOYADI, @KULLANICI_ADI, @KULLANICI_SIFRE,@KULLANICI_EMAIL, @KULLANICI_TEL, @KULLANICI_FIRMA_ID, @KULLANICI_DURUM_ID, @KULLANICI_CREATED_AT, @KULLANICI_CREATED_BY);  SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@KULLANICI_ADI", kullanici.KULLANICI_ADSOYAD.milToString());
                    cmd.Parameters.AddWithValue("@KULLANICI_EMAIL", kullanici.EMAIL.milToString());
                    cmd.Parameters.AddWithValue("@KULLANICI_ADI", kullanici.KULLANICI_ADI.milToString());
                    cmd.Parameters.AddWithValue("@KULLANICI_SIFRE", EncryptionAndDecryption.Encrypt(kullanici.SIFRE.milToString()));
                    cmd.Parameters.AddWithValue("@KULLANICI_TEL", kullanici.TEL.milToString());
                    cmd.Parameters.AddWithValue("@KULLANICI_FIRMA_ID", kullanici.FIRMA_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@KULLANICI_DURUM_ID", 1);
                    cmd.Parameters.AddWithValue("@KULLANICI_CREATED_AT", DateTime.Now.milToDateTime());
                    cmd.Parameters.AddWithValue("@KULLANICI_CREATED_BY", kullanici.CREATED_BY.milToInt32());

                    id = cmd.ExecuteScalar().milToInt32();

                    con.Close();
                }
            }
            insertedKullanici = GetById(id);
            return insertedKullanici;
        }

        internal KULLANICI Update(KULLANICI kullanici)
        {
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"UPDATE TKULLANICI SET
                KULLANICI_ADSOYAD = @KULLANICI_ADSOYAD, KULLANICI_ADI = @KULLANICI_ADI, KULLANICI_EMAIL = @KULLANICI_EMAIL, KULLANICI_TEL = @KULLANICI_TEL, KULLANICI_FIRMA_ID = @KULLANICI_FIRMA_ID, KULLANICI_DURUM_ID = @KULLANICI_DURUM_ID, KULLANICI_EDITED_AT = @KULLANICI_EDITED_AT, KULLANICI_EDITED_BY = @KULLANICI_EDITED_BY WHERE KULLANICI_ID = @KULLANICI_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@KULLANICI_ADSOYAD", kullanici.KULLANICI_ADSOYAD.milToString());
                    cmd.Parameters.AddWithValue("@KULLANICI_ADI", kullanici.KULLANICI_ADI.milToString());
                    cmd.Parameters.AddWithValue("@KULLANICI_EMAIL", kullanici.EMAIL.milToString());
                    cmd.Parameters.AddWithValue("@KULLANICI_TEL", kullanici.TEL.milToString());
                    cmd.Parameters.AddWithValue("@KULLANICI_FIRMA_ID", kullanici.FIRMA_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@KULLANICI_DURUM_ID", 1);
                    cmd.Parameters.AddWithValue("@KULLANICI_EDITED_AT", DateTime.Now.milToDateTime());
                    cmd.Parameters.AddWithValue("@KULLANICI_EDITED_BY", kullanici.EDITED_BY.milToInt32());
                    cmd.Parameters.AddWithValue("@KULLANICI_ID", kullanici.ID.milToInt32());

                    cmd.ExecuteNonQuery();

                    con.Close();
                }
            }

            kullanici = GetById(kullanici.ID);
            return kullanici;
        }

        internal bool Delete(int id)
        {
            bool result = false;
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"DELETE FROM TKULLANICI WHERE KULLANICI_ID = @KULLANICI_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@KULLANICI_ID", id);

                    cmd.ExecuteNonQuery();
                    result = true;

                    con.Close();
                }
            }
            return result;
        }
    }
}