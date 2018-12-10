IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQPQMailerFormLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQPQMailerFormLoad]
GO

	-- Author :	Tejashree Patil on 7 Jan 2013
-- Description:	Getting details on PQEMail form load
-- Modified By: Nilesh Utture on 17th June, 2013 Added MailerName & MailerEmailId in SELECT statement
-- EXEC TC_INQPQMailerFormLoad 5,600,1
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQPQMailerFormLoad]
@BranchId BIGINT, 
@CustId BIGINT,
@TC_NewCarInquiriesId BIGINT
AS                
BEGIN          
	SELECT	CD.CustomerName,CD.Email AS CustomerEmail,
			U.UserName, U.Mobile AS UserMobile,
			D.Organization, C.Name AS CityName,
			IL.CarDetails, D.MailerName, D.MailerEmailId -- Modified By: Nilesh Utture on 17th June, 2013
	FROM	TC_CustomerDetails CD WITH(NOLOCK)
			INNER JOIN	TC_Users U WITH(NOLOCK) ON 
						U.BranchId=CD.BranchId 
			INNER JOIN	Dealers D WITH(NOLOCK) ON 
						D.ID=U.BranchId 
			INNER JOIN	TC_InquiriesLead IL WITH(NOLOCK) ON 
						IL.TC_CustomerId = CD.Id AND U.Id=IL.TC_UserId
			INNER JOIN	TC_NewCarInquiries NCI WITH(NOLOCK) ON 
						NCI.TC_InquiriesLeadId =IL.TC_InquiriesLeadId AND U.Id=IL.TC_UserId
			INNER JOIN	Cities C WITH(NOLOCK) ON 
						C.ID=D.CityId
	WHERE	CD.BranchId=@BranchId AND 
			CD.Id=@CustId AND
			NCI.TC_NewCarInquiriesId=@TC_NewCarInquiriesId	
			
END
