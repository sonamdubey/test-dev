IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ConsumerCreditPointDailyLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ConsumerCreditPointDailyLog]
GO

	
-- =============================================
-- Author:		Manish
-- Create date: 20 Oct 2015
-- Description: This SP will capture daily snapshot of ConsumerCreditPoints at the start of the day
-- -- =============================================
	CREATE PROCEDURE [dbo].[ConsumerCreditPointDailyLog]
	AS 
       BEGIN
			INSERT INTO [dbo].[ConsumerCreditPointDealerDailyLog]
																   (
																	 [AsOnDate],
																	 [Id],
																	 [ConsumerType],
																	 [ConsumerId],
																	 [Points],
																	 [ExpiryDate],
																	 [PackageType],
																	 [CustomerPackageId],
																	 [IsDealerPackageActive]
																   )
														 SELECT      CONVERT(DATE,GETDATE()),
																	 C.ID,
																	 C.ConsumerType,
																	 C.ConsumerId,
																	 C.Points,
																	 C.ExpiryDate,
																	 C.PackageType,
																	 C.CustomerPackageId,
																	 CASE WHEN D.[Status]=0 THEN 1
																		  WHEN D.[Status]=1 THEN 0 END   IsDealerPackageActive
														 FROM ConsumerCreditPoints AS C WITH (NOLOCK)
														 LEFT JOIN Dealers         AS D WITH (NOLOCK) ON C.ConsumerId =D.ID 
																							   AND C.ConsumerType=1
														 WHERE C.ConsumerType=1
        END 
