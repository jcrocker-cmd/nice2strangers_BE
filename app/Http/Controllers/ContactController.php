<?php

namespace App\Http\Controllers;

use App\Models\Contact;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Mail;
use Illuminate\Support\Facades\Log;

class ContactController extends Controller
{
    /**
     * Display a listing of the resource.
     */
    public function index()
    {
        //
    }

    /**
     * Show the form for creating a new resource.
     */
    public function create()
    {
        //
    }

    /**
     * Store a newly created resource in storage.
     */
    public function store(Request $request)
    {
        //
    }

    /**
     * Display the specified resource.
     */
    public function show(Contact $contact)
    {
        //
    }

    /**
     * Show the form for editing the specified resource.
     */
    public function edit(Contact $contact)
    {
        //
    }

    /**
     * Update the specified resource in storage.
     */
    public function update(Request $request, Contact $contact)
    {
        //
    }

    /**
     * Remove the specified resource from storage.
     */
    public function destroy(Contact $contact)
    {
        //
    }

    // public function sendEmail(Request $request)
    // {
    //     $data = $request->only(['name', 'email', 'subject', 'message']);

    //     Mail::send('email.contact', ['data' => $data], function ($message) use ($data) {
    //         $message->to('narbajajc@gmail.com')
    //             ->subject('Contact Form: ' . $data['subject'])
    //             ->replyTo($data['email'], $data['name']);
    //     });

    //     return response()->json(['message' => 'Email sent successfully']);
    // }

    public function sendEmail(Request $request)
    {
        try {
            $data = $request->only(['name', 'email', 'subject', 'message']);

            Mail::send('email.contact', ['data' => $data], function ($message) use ($data) {
                $message->to('narbajajc@gmail.com')
                    ->subject('Contact Form: ' . $data['subject']);
            });


            return response()->json(['message' => 'Email sent successfully', 'data' => $data]);
        } catch (\Exception $e) {
            \Log::error('Email error: ' . $e->getMessage());
            return response()->json(['error' => 'Failed to send email.'], 500);
        }
    }


    public function testSendEmail(Request $request)
    {
        try {
            Mail::raw('This is a test message.', function ($message) {
                $message->to('narbajajc@gmail.com')
                    ->subject('Test Subject from Laravel');
            });

            return response()->json(['message' => 'Email sent successfully']);
        } catch (\Exception $e) {
            \Log::error('Email error: ' . $e->getMessage());
            return response()->json(['error' => 'Failed to send email'], 500);
        }
    }
}
