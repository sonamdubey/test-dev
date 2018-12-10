IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[NCAlert].[InsertCustomerEligibleForMail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [NCAlert].[InsertCustomerEligibleForMail]
GO

	
--Created By: Manish on 20-05-2014
--Description: Send the email alert for the customer who have take PQ logic provided by Moupiya.
--- Modified by/ Approved by : Manish on 01-07-2014 for considering all makes
--  Modified by Manish on 19-10-2015 added condition IsMetallic=0 for CW_NewCarShowroomPrices.

CREATE PROCEDURE [NCAlert].[InsertCustomerEligibleForMail]
AS
BEGIN
 
       DECLARE  @Date datetime= GETDATE()-1

	   SET  @Date= convert(datetime,convert(varchar(10),@Date,120)+ ' 00:00:00')

        TRUNCATE TABLE NCAlert.CustomerEligibleForMail;

		 WITH CTE AS 
					(   SELECT N.CustomerId CustomerId
						FROM NewCarPurchaseInquiries AS N WITH (NOLOCK) 
						JOIN VWMMV                   AS V  WITH (NOLOCK)  ON V.VersionId=N.CarVersionId
						WHERE  N.ReqDateTimeDatePart=@Date
						 --  AND V.MakeId IN (7,8,10)  ---condition commented by Manish on 01-07-2014 	
						 GROUP BY N.CustomerId HAVING COUNT(*)=1
					  ) 
					  INSERT INTO NCAlert.CustomerEligibleForMail
															   (CustomerID,
																NewCarPurchaseInquiriesId,
																PQVersionId,
																CustomerName,
																CustomerEmail,
																CustomerMobile,
																PQRequestDate,
																IsActive,
																CustomerKey,
																CityId,
																CityName)
			  									  SELECT       C.ID CustomerId,
															   N.ID NewCarPurchaseInquiriesId,
															   N.CarVersionId PQVersionId,
															   C.Name       CustomerName,
															   C.email      CustomerEmail,
															   C.Mobile     CustomerMobile,
															   N.RequestDateTime PQRequestDate,
															   1   IsActive,
															   CSK.CustomerKey CustomerKey,
															   CT.ID,
															   CT.Name
                                                   FROM  NewCarPurchaseInquiries AS N WITH (NOLOCK) 
													JOIN CTE WITH (NOLOCK)     ON  CTE.CustomerId=N.CustomerId
													JOIN NewPurchaseCities AS NPC WITH (NOLOCK) ON NPC.InquiryId=N.Id
												    JOIN Cities AS CT WITH (NOLOCK) ON CT.Id=NPC.CityId
													JOIN Customers  AS C WITH (NOLOCK) ON C.Id=CTE.CustomerId
													JOIN CustomerSecurityKey AS CSK WITH (NOLOCK) ON CSK.CustomerId=C.ID
													WHERE N.ReqDateTimeDatePart=@Date
													  AND C.IsFake=0;

                       ---  Modified by Manish on 19-10-2015 added condition IsMetallic=0 for CW_NewCarShowroomPrices.
				UPDATE CU SET ShowroomPrice=(SELECT TOP 1 PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=CU.PQVersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=2 AND C.isMetallic=0),					
									   RTO=(SELECT TOP 1 PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=CU.PQVersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=3 AND C.isMetallic=0),
								 Insurance=(SELECT TOP 1 PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=CU.PQVersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=5 AND C.isMetallic=0),
							  DepotCharges=(SELECT TOP 1 PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=CU.PQVersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=26 AND C.isMetallic=0)
				FROM NCAlert.CustomerEligibleForMail AS CU WITH (NOLOCK);


				UPDATE CU SET MT1ShowroomPrice=(SELECT TOP 1 PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=D.MT1VersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=2 AND C.isMetallic=0 ),					
									   MT1RTO=(SELECT TOP 1 PQ_CategoryItemValue FROM  CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=D.MT1VersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=3 AND C.isMetallic=0 ),
								 MT1Insurance=(SELECT TOP 1 PQ_CategoryItemValue FROM  CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=D.MT1VersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=5 AND C.isMetallic=0 ),
							  MT1DepotCharges=(SELECT TOP 1 PQ_CategoryItemValue FROM  CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=D.MT1VersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=26 AND C.isMetallic=0)
				FROM NCAlert.CustomerEligibleForMail AS CU WITH (NOLOCK)
				JOIN NCAlert.NewCarAlertEmailEntireCarData AS D WITH (NOLOCK) ON CU.PQVersionId=D.VersionId;



				UPDATE CU SET MT2ShowroomPrice=(SELECT TOP 1 PQ_CategoryItemValue FROM  CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=D.MT2VersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=2 AND C.isMetallic=0 ),					
									   MT2RTO=(SELECT TOP 1  PQ_CategoryItemValue FROM   CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=D.MT2VersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=3 AND C.isMetallic=0),
								 MT2Insurance=(SELECT TOP 1 PQ_CategoryItemValue FROM   CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=D.MT2VersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=5 AND C.isMetallic=0),
							  MT2DepotCharges=(SELECT TOP 1 PQ_CategoryItemValue FROM   CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=D.MT2VersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=26 AND C.isMetallic=0)
				FROM NCAlert.CustomerEligibleForMail AS CU WITH (NOLOCK)
				JOIN NCAlert.NewCarAlertEmailEntireCarData AS D WITH (NOLOCK) ON CU.PQVersionId=D.VersionId;

				UPDATE CU SET MT3ShowroomPrice=(SELECT TOP 1 PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=D.MT3VersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=2  AND C.isMetallic=0),					
									   MT3RTO=(SELECT TOP 1 PQ_CategoryItemValue FROM  CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=D.MT3VersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=3  AND C.isMetallic=0),
								 MT3Insurance=(SELECT TOP 1 PQ_CategoryItemValue FROM  CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=D.MT3VersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=5  AND C.isMetallic=0),
							  MT3DepotCharges=(SELECT TOP 1 PQ_CategoryItemValue FROM  CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=D.MT3VersionId AND C.CityId=CU.CityId AND C.PQ_CategoryItem=26 AND C.isMetallic=0)
				FROM NCAlert.CustomerEligibleForMail AS CU WITH (NOLOCK)
				JOIN NCAlert.NewCarAlertEmailEntireCarData AS D WITH (NOLOCK) ON CU.PQVersionId=D.VersionId;


 END 