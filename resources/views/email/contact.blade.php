<!-- <div>

    <h2>New Contact Form Message</h2>
    <p><strong>Name:</strong> {{ $data['name'] }}</p>
    <p><strong>Email:</strong> {{ $data['email'] }}</p>
    <p><strong>Subject:</strong> {{ $data['subject'] }}</p>
    <p><strong>Message:</strong></p>
    <p>{{ $data['message'] }}</p>

</div> -->


<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <title>New Contact Message</title>
</head>

<body style="margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;">
    <table width="100%" cellpadding="0" cellspacing="0" style="padding: 30px 0;">
        <tr>
            <td align="center">
                <table width="600" cellpadding="0" cellspacing="0"
                    style="background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 10px rgba(0,0,0,0.1);">
                    <!-- Logo Header -->
                    <tr>
                        <td style="background-color: #FBD241; padding: 20px; text-align: center;">
                            <img src="https://res.cloudinary.com/dnh4lkqlw/image/upload/v1747454491/logo_yoqhzt.png"
                                alt="Your Logo" style="max-width: 70px;">
                        </td>
                    </tr>

                    <!-- Content -->
                    <tr>
                        <td style="padding: 30px;">
                            <h2 style="color: #333;">New Contact Form Message</h2>

                            <p><strong style="color: #555;">Name:</strong> {{ $data['name'] }}</p>
                            <p><strong style="color: #555;">Email:</strong> {{ $data['email'] }}</p>
                            <p><strong style="color: #555;">Subject:</strong> {{ $data['subject'] }}</p>
                            <p><strong style="color: #555;">Message:</strong></p>
                            <p style="color: #333;">{{ $data['message'] }}</p>
                        </td>
                    </tr>

                    <!-- Footer -->
                    <tr>
                        <td
                            style="background-color: #f0f0f0; text-align: center; padding: 20px; font-size: 12px; color: #777;">
                            &copy; {{ date('Y') }} nice2strangers. All rights reserved.
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
</body>

</html>