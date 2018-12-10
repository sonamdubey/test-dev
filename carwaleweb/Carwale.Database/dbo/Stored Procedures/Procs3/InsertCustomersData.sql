IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertCustomersData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertCustomersData]
GO

	


CREATE PROCEDURE [dbo].[InsertCustomersData]( @CustomerCallingDetail CustomerCallingDetail READONLY)
AS
 BEGIN
     INSERT INTO DCRM_CustomerCalling
                 (CustomerId,
				  InquiryDate,	                
                  EntryDate,
                  EnteredBy,
                  CCExecutive)
     SELECT CCD.CustomerId,
            CCD.InquiryDate,
            CCD.EntryDate,
            CCD.EnteredBy,
            CCD.CExecutive            
    FROM @CustomerCallingDetail CCD
  END 
