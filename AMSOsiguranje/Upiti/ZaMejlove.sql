SELECT MAX ([ID]) AS ID FROM [NotificationsDB].[mail].[MailDeliveryStatus];

SELECT * FROM [NotificationsDB].[mail].[MailDeliveryStatus] ORDER BY [ID] DESC;

SELECT * FROM [NotificationsDB].[mail].[MailAttachments] ORDER BY [IDMail] DESC;

SELECT  * FROM [NotificationsDB].[mail].[MailHeaders] ORDER BY [IdMail] DESC;

SELECT [NotificationsDB].[mail].[MailDeliveryStatus].*, [NotificationsDB].[mail].[MailHeaders].[Subject]  
FROM [NotificationsDB].[mail].[MailDeliveryStatus] 
INNER JOIN [NotificationsDB].[mail].[MailHeaders] ON [NotificationsDB].[mail].[MailDeliveryStatus].[IDMail] = [NotificationsDB].[mail].[MailHeaders].[IDMail]
ORDER BY [ID] DESC;
