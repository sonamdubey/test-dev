IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CustomerSearch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CustomerSearch]
GO

	
-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 29-Apr-14
-- Description:	Get the Search result for customer
-- Modified By Vivek gupta, on 29/05/2014, Changes where conditions
-- =============================================
CREATE PROCEDURE [dbo].[TC_CustomerSearch]
@CusMobile VARCHAR(15) = NULL,
@BranchId INT = NULL,
@ChassisNumber VARCHAR(100) = NULL,
@UniqueId VARCHAR(12) = NULL
--@FromIndex INT,
--@ToIndex INT
AS
BEGIN

	SET NOCOUNT ON;  
	-- WITH Cte1 
        --   AS (
		IF @ChassisNumber IS NOT  NULL
			BEGIN
			   SELECT  DISTINCT  TC.id AS CusId, TC.CustomerName, TC.Mobile, TC.Email--,TIl.TC_InquiriesLeadId, TNC.TC_NewCarInquiriesId,TNB.TC_NewCarBookingId, TC.BranchId,TC.UniqueCustomerId,TNB.ChassisNumber
				FROM   TC_CustomerDetails  AS TC  WITH (NOLOCK) 
				JOIN TC_InquiriesLead AS TIL   WITH (NOLOCK) ON  TC.id = TIL.TC_CustomerId
				JOIN TC_NewCarInquiries AS TNC WITH (NOLOCK) ON TNC.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId
				LEFT JOIN TC_NewCarBooking AS TNB WITH (NOLOCK) ON TNB.TC_NewCarInquiriesId = TNC.TC_NewCarInquiriesId
				WHERE    (TNB.ChassisNumber = @ChassisNumber 
						OR   (TC.Mobile = @CusMobile  AND TC.BranchId = @BranchId AND TC.IsleadActive=1) 
						OR   (TC.UniqueCustomerId = @UniqueId AND TC.IsleadActive=1)				           
						 )
					--AND TC.IsleadActive=1
			END
		ELSE
			SELECT  DISTINCT  TC.id AS CusId, TC.CustomerName, TC.Mobile, TC.Email--,TIl.TC_InquiriesLeadId, TNC.TC_NewCarInquiriesId,TNB.TC_NewCarBookingId, TC.BranchId,TC.UniqueCustomerId,TNB.ChassisNumber
			FROM   TC_CustomerDetails  AS TC  WITH (NOLOCK) 
			WHERE   ((TC.Mobile = @CusMobile  AND TC.BranchId = @BranchId) 
						OR  TC.UniqueCustomerId = @UniqueId) 				           
				 AND TC.IsleadActive=1

END
 

